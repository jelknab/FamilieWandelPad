using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Map;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.Pages;
using Mapsui.Styles;
using Mapsui.UI.Forms;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Color = Mapsui.Styles.Color;

namespace FamilieWandelPad.navigation
{
    public class Navigator : INavigator
    {
        private readonly IGeolocator _geoLocator;
        private readonly INavigationMap _mapView;
        private readonly Distance _wayPointTriggerDistance = Distance.FromMeters(5);
        private readonly Distance _maxOffCourseDistance = Distance.FromMeters(20);
        private readonly Distance _maxSkippingDistance = Distance.FromMeters(500);


        private readonly Route _route;
        private PositionLayer _expectedPositionLayer;
        private PositionLayer _positionLayer;

        private WalkedPathLayer _walkedPathLayer;

        public Navigator(INavigationMap mapView, Route route, IGeolocator geoLocator)
        {
            _mapView = mapView;
            _route = route;
            _geoLocator = geoLocator;
        }

        private IEnumerator<RoutePoint> RouteEnumerator { get; set; }
        private List<RoutePoint> VisitedWaypoints { get; set; }

        private Direction NavigationDirection { get; set; }
        public RoutePoint LastWaypoint { get; private set; }
        public RoutePoint NextWaypoint { get; private set; }

        public async Task StartNavigation()
        {
            var position = (await _geoLocator.GetPositionAsync()).ToGeoPosition();

            LastWaypoint = _route.FindClosestWaypoint(position);

            var section = _route.GetWaypointSection(LastWaypoint);
            NavigationDirection = _route.DetermineLongestDirectionInSection(LastWaypoint, section);

            RouteEnumerator = _route.GetEnumerable(LastWaypoint, NavigationDirection).GetEnumerator();
            RouteEnumerator.MoveNext();
            ActivateNextWaypoint();

            InitializeMap(position);

            await StartListeningToLocationAsync();
        }

        public void OnLocationUpdate(object sender, PositionEventArgs e)
        {
            var position = e.Position.ToGeoPosition();

            var expectedPosition = MapExtensions.ClosestPositionBetweenPoints(LastWaypoint, NextWaypoint, position);
            var isOnRoute = IsOnRoute(expectedPosition, position);

            if (!isOnRoute)
            {
                var possibleSkip = GetNextPointOnRoute(position);
                if (possibleSkip != null)
                {
                    do
                    {
                        ActivateNextWaypoint();
                        VisitedWaypoints.Add(LastWaypoint);
                    } while (LastWaypoint != possibleSkip);
                }
            }

            if (HasArrivedAtWaypoint(position))
            {
                ActivateNextWaypoint();
                VisitedWaypoints.Add(LastWaypoint);

                if (LastWaypoint is PointOfInterest poi) ShowPointOfInterestModal(poi);
            }

            UpdateMap(position, expectedPosition, isOnRoute);
        }

        private RoutePoint GetNextPointOnRoute(GeoPosition position)
        {
            var last = LastWaypoint;
            var skipDistance = 0d;
            foreach (var routePoint in _route.GetEnumerable(NextWaypoint, NavigationDirection))
            {
                var expectedPosition = MapExtensions.ClosestPositionBetweenPoints(last, routePoint, position);
                skipDistance += last.Distance(routePoint);

                if (VisitedWaypoints.Contains(routePoint) || skipDistance > _maxSkippingDistance.Miles) break;

                if (IsOnRoute(expectedPosition, position))
                {
                    return routePoint;
                }
                
                last = routePoint;
            }

            return null;
        }

        public void OnNavigationFinished()
        {
            throw new NotImplementedException();
        }

        private void InitializeMap(GeoPosition position)
        {
            VisitedWaypoints = new List<RoutePoint> {LastWaypoint};
            _walkedPathLayer = new WalkedPathLayer(VisitedWaypoints, Consts.WalkedPathLayerName);
            _mapView.AddLayer(_walkedPathLayer);

            _positionLayer = new PositionLayer(position, new SymbolStyle
            {
                Fill = new Brush(Color.FromString("#3366CC")),
                SymbolScale = 0.25,
                SymbolType = SymbolType.Ellipse
            });
            _mapView.AddLayer(_positionLayer);

            _expectedPositionLayer = new PositionLayer(LastWaypoint, new SymbolStyle
            {
                BitmapId = GetBitMap("FamilieWandelPad.Assets.navigationArrow.svg"),
                SymbolScale = 0.075,
                RotateWithMap = true
            });
            _mapView.AddLayer(_expectedPositionLayer);
        }

        private int GetBitMap(string path)
        {
            var bitmapStream = typeof(Navigator).GetTypeInfo().Assembly.GetManifestResourceStream(path);
            return BitmapRegistry.Instance.Register(bitmapStream);
        }

        private async Task StartListeningToLocationAsync()
        {
            if (!_geoLocator.IsListening)
                await _geoLocator.StartListeningAsync(TimeSpan.FromSeconds(2.5), 2, true);

            _geoLocator.PositionChanged += OnLocationUpdate;
        }

        protected virtual void ShowPointOfInterestModal(PointOfInterest poi)
        {
            var poiPage = new PointOfInterestPage(poi);
            Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(poiPage));
        }

        private void UpdateMap(GeoPosition position, GeoPosition expectedPosition, bool onTrack)
        {
            var rotation = LastWaypoint.DegreeBearing(NextWaypoint);
            if (onTrack)
                _mapView.CenterView(expectedPosition, -rotation);
            else
                _mapView.CenterView(position, 0);


            _walkedPathLayer.UpdatePath(VisitedWaypoints.Append(expectedPosition));

            _positionLayer.Update(position, 0);
            _expectedPositionLayer.Update(expectedPosition, rotation);
        }


        private void ActivateNextWaypoint()
        {
            LastWaypoint = RouteEnumerator.Current;
            RouteEnumerator.MoveNext();
            NextWaypoint = RouteEnumerator.Current;
        }

        private bool IsOnRoute(GeoPosition expected, GeoPosition actual)
        {
            return actual.Distance(expected) < _maxOffCourseDistance.Miles;
        }

        private bool HasArrivedAtWaypoint(GeoPosition position)
        {
            return position.Distance(NextWaypoint) < _wayPointTriggerDistance.Miles;
        }
    }
}