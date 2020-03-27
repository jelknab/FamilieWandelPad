using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Map;
using FamilieWandelPad.Map.MapLayers;
using Mapsui.Styles;
using Mapsui.UI.Forms;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.navigation
{
    public class Navigator : INavigator
    {
        private readonly IGeolocator _geoLocator;
        private readonly INavigationMap _mapView;
        private readonly Distance _maxOffCourseDistance = Distance.FromMeters(20);


        private readonly Route _route;
        private readonly Distance _wayPointTriggerDistance = Distance.FromMeters(10);
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
            NextWaypoint = LastWaypoint;

            var section = _route.GetWaypointSection(NextWaypoint);
            NavigationDirection = _route.DetermineLongestDirectionInSection(NextWaypoint, section);

            RouteEnumerator = _route.GetEnumerable(LastWaypoint, NavigationDirection).GetEnumerator();
            RouteEnumerator.MoveNext();
            ActivateNextWaypoint();

            InitializeMap(position);

            await StartListeningToLocationAsync();
        }

        public void OnLocationUpdate(object sender, PositionEventArgs e)
        {
            var position = e.Position.ToGeoPosition();

            var expectedPosition = MapExtensions.ClosestPositionBetweenPoints(
                LastWaypoint,
                NextWaypoint,
                position
            );
            var onTrack = IsUserOnTrack(expectedPosition, position);

            if (!onTrack)
            {
                //todo: warn user
            }

            if (HasArrivedAtWaypoint(position))
            {
                ActivateNextWaypoint();

                VisitedWaypoints.Add(LastWaypoint);

                if (LastWaypoint is PointOfInterest) ShowPointOfInterestModal(LastWaypoint as PointOfInterest);
            }

            UpdateMap(position, expectedPosition, onTrack);
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

        private void ShowPointOfInterestModal(PointOfInterest poi)
        {
            //todo: Display point of interest
        }

        private void UpdateMap(GeoPosition position, GeoPosition expectedPosition, bool onTrack)
        {
            var rotation = LastWaypoint.AngleTo(NextWaypoint);
            if (onTrack)
                _mapView.CenterView(expectedPosition, rotation);
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

        private bool IsUserOnTrack(GeoPosition expected, GeoPosition actual)
        {
            return actual.Distance(expected) < _maxOffCourseDistance.Miles;
        }

        private bool HasArrivedAtWaypoint(GeoPosition position)
        {
            return position.Distance(NextWaypoint) < _wayPointTriggerDistance.Miles;
        }
    }
}