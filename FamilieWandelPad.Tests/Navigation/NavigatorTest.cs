using System.Threading.Tasks;
using FamilieWandelPad.Map;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Tests.Map;
using FamilieWandelPad.Tests.Navigation.Route;
using Plugin.Geolocator.Abstractions;
using Xunit;

namespace FamilieWandelPad.Tests.Navigation
{
    public class NavigatorTest
    {
        private readonly FamilieWandelPad.Navigation.Route.Route _route = new RouteStub();

        [Fact]
        public async Task NextChangeTest()
        {
            var geoLocator = new CrossGeoLocatorMock(_route.Waypoints[3]);
            var navigator = new Navigator(new MockMapsUiView(), _route, geoLocator);

            await navigator.StartNavigation();
            
            // Walk till index overflow, test route progressing
            Assert.Equal(_route.Waypoints[3], navigator.LastWaypoint);
            Assert.Equal(_route.Waypoints[2], navigator.NextWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[2]);
            Assert.Equal(_route.Waypoints[2], navigator.LastWaypoint);
            Assert.Equal(_route.Waypoints[1], navigator.NextWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[1]);
            Assert.Equal(_route.Waypoints[1], navigator.LastWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[0]);
            Assert.Equal(_route.Waypoints[0], navigator.LastWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[^1]);
            Assert.Equal(_route.Waypoints[^1], navigator.LastWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[^2]);
            Assert.Equal(_route.Waypoints[^2], navigator.LastWaypoint);
            
            // Test next waypoint not triggering if not close
            geoLocator.UpdatePosition(MapExtensions.InterpolatePosition(_route.Waypoints[^1], _route.Waypoints[^2], 0.45));
            Assert.Equal(_route.Waypoints[^2], navigator.LastWaypoint);
        }
    }
}