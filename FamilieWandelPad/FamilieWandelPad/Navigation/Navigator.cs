using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilieWandelPad.Map;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.Navigation.Route;
using FamilieWandelPad.Navigation.Route.waypoints;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace FamilieWandelPad.navigation
{
    public class Navigator : INavigator
    {
        private readonly Distance _maxOffCourseDistance = Distance.FromMeters(10);
        private readonly Distance _wayPointTriggerDistance = Distance.FromMeters(5);

        
        private readonly Route _route;
        private readonly IGeolocator _geoLocator;
        private readonly INavigationMap _mapView;

        private IEnumerator<WayPoint> RouteEnumerator { get; set; }
        private List<WayPoint> VisitedWaypoints { get; set; }
        
        private WalkedPathLayer _walkedPathLayer;
        private CurrentPositionLayer _positionLayer;
        
        private Direction NavigationDirection { get; set; }
        public WayPoint LastWaypoint { get; private set; }
        public WayPoint NextWaypoint { get; private set; }

        public Navigator(INavigationMap mapView, Route route, IGeolocator geoLocator)
        {
            _mapView = mapView;
            _route = route;
            _geoLocator = geoLocator;
        }

        public async Task StartNavigation()
        {
            var position = await _geoLocator.GetPositionAsync();
            
            LastWaypoint = _route.FindClosestWaypoint(position);
            NextWaypoint = LastWaypoint;
            
            var section = _route.GetWaypointSection(NextWaypoint);
            NavigationDirection = _route.DetermineLongestDirectionInSection(NextWaypoint, section);

            RouteEnumerator = _route.GetEnumerable(LastWaypoint, NavigationDirection).GetEnumerator();
            RouteEnumerator.MoveNext();

            InitializeMap(position);
            
            await StartListeningToLocationAsync();
        }

        private void InitializeMap(Position position)
        {
            VisitedWaypoints = new List<WayPoint>() {LastWaypoint};
            _walkedPathLayer = new WalkedPathLayer(VisitedWaypoints, Consts.WalkedPathLayerName);
            _mapView.AddLayer(_walkedPathLayer);
            
            _positionLayer = new CurrentPositionLayer(position);
            _mapView.AddLayer(_positionLayer);
        }

        private async Task StartListeningToLocationAsync()
        {
            if (!_geoLocator.IsListening)
                await _geoLocator.StartListeningAsync(TimeSpan.FromSeconds(2.5), 2, true);
            
            _geoLocator.PositionChanged += OnLocationUpdate;
        }

        public void OnLocationUpdate(object sender, PositionEventArgs e)
        {
            var position = e.Position;

            var expectedPosition = MapExtensions.ClosestPositionBetweenPoints(LastWaypoint, NextWaypoint, position);

            if (!IsUserOnTrack(expectedPosition, position))
            {
                //todo: warn user
                _mapView.CenterView(position);
            }
            else
            {
                _mapView.CenterView(expectedPosition);
            }

            if (HasArrivedAtWaypoint(position))
            {
                ActivateNextWaypoint();
                
                VisitedWaypoints.Add(LastWaypoint);
                LastWaypoint.OnArrival();
            }

            UpdateMap(position, expectedPosition);
        }

        private void UpdateMap(Position position, Position expectedPosition)
        {
            _walkedPathLayer.UpdatePath(expectedPosition);
            _positionLayer.Update(position, 0);
            
            _mapView.Update();
        }

        private void ActivateNextWaypoint()
        {
            LastWaypoint = RouteEnumerator.Current;
            RouteEnumerator.MoveNext();
            NextWaypoint = RouteEnumerator.Current;
        }

        private bool IsUserOnTrack(Position expected, Position actual)
        {
            return actual.CalculateDistance(expected) < _maxOffCourseDistance.Miles;
        }

        private bool HasArrivedAtWaypoint(Position position)
        {
            return position.CalculateDistance(NextWaypoint) < _wayPointTriggerDistance.Miles;
        }

        public void OnNavigationFinished()
        {
            throw new NotImplementedException();
        }
    }
}