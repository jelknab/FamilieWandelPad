using System.Threading.Tasks;
using FamilieWandelPad.Map;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Tests.Map;
using FamilieWandelPad.Tests.Navigation.Route;
using Xunit;

namespace FamilieWandelPad.Tests.Navigation
{
    public class NavigatorTest
    {
        private readonly Database.Model.Route _route = new RouteStub();

        [Fact]
        public async Task NextChangeTest()
        {
            var geoLocator = new CrossGeoLocatorMock(_route.Waypoints[3].ToGeoLocatorPosition());
            var navigator = new NavigatorMockSpy(new MockMapsUiView(), _route, geoLocator);

            await navigator.StartNavigation();

            // Walk till index overflow, test route progressing
            Assert.Equal(_route.Waypoints[3], navigator.LastWaypoint);
            Assert.Equal(_route.Waypoints[2], navigator.NextWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[2].ToGeoLocatorPosition());
            Assert.Equal(_route.Waypoints[2], navigator.LastWaypoint);
            Assert.Equal(_route.Waypoints[1], navigator.NextWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[1].ToGeoLocatorPosition());
            Assert.Equal(_route.Waypoints[1], navigator.LastWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[0].ToGeoLocatorPosition());
            Assert.Equal(_route.Waypoints[0], navigator.LastWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[^1].ToGeoLocatorPosition());
            Assert.Equal(_route.Waypoints[^1], navigator.LastWaypoint);
            geoLocator.UpdatePosition(_route.Waypoints[^2].ToGeoLocatorPosition());
            Assert.Equal(_route.Waypoints[^2], navigator.LastWaypoint);
            
            // Test if want to show modal for POI
            Assert.True(navigator.ShowPOICalled);

            // Test next waypoint not triggering if not close
            geoLocator.UpdatePosition(
                MapExtensions.InterpolatePosition(_route.Waypoints[^1], _route.Waypoints[^2], 0.45)
                    .ToGeoLocatorPosition()
            );
            Assert.Equal(_route.Waypoints[^2], navigator.LastWaypoint);
        }
    }
}