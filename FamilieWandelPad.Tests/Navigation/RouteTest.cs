using System.Collections;
using System.Collections.Generic;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Navigation.Route.waypoints;
using FamilieWandelPad.Tests.Navigation.Route;
using Plugin.Geolocator.Abstractions;
using Xunit;

namespace FamilieWandelPad.Tests.Navigation
{
    public class RouteTest
    {
        private readonly FamilieWandelPad.Navigation.Route.Route _route = new RouteStub();

        [Fact]
        public void FindClosestPointTest()
        {
            foreach (var waypoint in _route.Waypoints)
            {
                var startingWaypoint = _route.FindClosestWaypoint(new Position(waypoint.Latitude, waypoint.Longitude));
                Assert.Equal(waypoint, startingWaypoint);
            }
        }
        
        [Theory]
        [InlineData(52.21958, 4.56069, "Buitenkaag")]
        [InlineData(52.21901, 4.56000, "Buitenkaag")]
        [InlineData(52.21837, 4.55974, "Kaag")]
        public void SectionDetectionTest(double latitude, double longitude, string sectionName)
        {
            var section = _route.GetWaypointSection(new WayPoint(latitude, longitude));
            
            Assert.Equal(sectionName, section.Name);
        }

        [Fact]
        public void NoSectionsTest()
        {
            FamilieWandelPad.Navigation.Route.Route route = new FamilieWandelPad.Navigation.Route.Route();

            var section = route.GetWaypointSection(new WayPoint(0, 0));
            
            Assert.Equal("Default", section.Name);
        }

        [Theory]
        [ClassData(typeof(DirectionTestData))]
        public void DirectionSelectionTest(int waypointIndex, Direction assertedDirection)
        {
            var waypoint = _route.Waypoints[waypointIndex];
            var section = _route.GetWaypointSection(waypoint);
            var direction = _route.DetermineLongestDirectionInSection(waypoint, section);
            
            Assert.Equal(assertedDirection, direction);
        }
        
    }

    public class DirectionTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {0, Direction.Forward};
            yield return new object[] {3, Direction.Backward};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}