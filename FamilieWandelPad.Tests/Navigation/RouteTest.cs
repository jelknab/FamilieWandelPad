using System.Collections;
using System.Collections.Generic;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Tests.Navigation.Route;
using Xunit;

namespace FamilieWandelPad.Tests.Navigation
{
    public class RouteTest
    {
        private readonly Database.Model.Route _route = new RouteStub();

        [Theory]
        [InlineData(52.21958, 4.56069, "Buitenkaag")]
        [InlineData(52.21901, 4.56000, "Buitenkaag")]
        [InlineData(52.21837, 4.55974, "Kaag")]
        public void SectionDetectionTest(double latitude, double longitude, string sectionName)
        {
            var section = _route.GetSectionForPosition(new WayPoint(latitude, longitude));

            Assert.Equal(sectionName, section.Name);
        }

        [Theory]
        [ClassData(typeof(DirectionTestData))]
        public void DirectionSelectionTest(int waypointIndex, Direction assertedDirection)
        {
            var waypoint = _route.Waypoints[waypointIndex];
            var section = _route.GetSectionForPosition(waypoint);
            var direction = _route.DetermineLongestDirectionInSection(waypoint, section);

            Assert.Equal(assertedDirection, direction);
        }

        [Fact]
        public void FindClosestPointTest()
        {
            foreach (var waypoint in _route.Waypoints)
            {
                var startingWaypoint = _route.FindClosestWaypoint(waypoint);
                Assert.Equal(waypoint, startingWaypoint);
            }
        }

        [Fact]
        public void NoSectionsTest()
        {
            var route = new Database.Model.Route
            {
                Waypoints = new List<RoutePoint> {new WayPoint(0, 0)},
                Sections = null
            };

            var section = route.GetSectionForPosition(new WayPoint(0, 0));

            Assert.Equal("Default", section.Name);
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