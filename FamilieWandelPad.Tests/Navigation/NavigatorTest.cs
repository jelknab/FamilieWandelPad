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
            var navigator = new NavigatorMockSpy(new MockMapsUiView(), _route, geoLocator, new NavigationStats(_route));

            await navigator.StartNavigation();
            
            foreach (var routePoint in _route.GetEnumerable(_route.Waypoints[3], Direction.Backward))
            {
                geoLocator.UpdatePosition(routePoint.ToGeoLocatorPosition());
                Assert.Equal(routePoint, navigator.LastWaypoint);
            }

            Assert.True(navigator.ShowPOICalled);
            Assert.True(navigator.NavigationFinishedCalled);
        }
    }
}