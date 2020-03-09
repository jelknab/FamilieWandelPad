
using System;
using System.Threading.Tasks;
using FamilieWandelPad.Navigation.Route.waypoints;
using Mapsui.Forms.Extensions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms.Maps;
using Position = Xamarin.Forms.Maps.Position;

namespace FamilieWandelPad.navigation
{
    class Navigator : INavigator
    {
        private readonly MapsUIView _mapControl;

        public Navigator(MapsUIView mapControl)
        {
            _mapControl = mapControl;
            StartListeningToLocationAsync();
        }

        private async Task StartListeningToLocationAsync()
        {
            if (await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 2, true))
            {
                CrossGeolocator.Current.PositionChanged += OnLocationUpdate;
            }
        }

        public WayPoint FindStartingWaypoint()
        {
            throw new NotImplementedException();
        }

        public WayPoint GetNextWaypoint(WayPoint currentWayPoint)
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(WayPoint target)
        {
            throw new NotImplementedException();
        }

        public void OnNavigationFinished()
        {
            throw new NotImplementedException();
        }

        public void OnLocationUpdate(object sender, PositionEventArgs e)
        {
            var location = e.Position;
            _mapControl.NativeMap.NavigateTo(new Position(location.Latitude, location.Longitude).ToMapsui());
        }
    }
}
