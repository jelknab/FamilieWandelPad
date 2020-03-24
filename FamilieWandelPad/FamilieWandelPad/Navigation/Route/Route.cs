using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Navigation.Route.waypoints;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Navigation.Route
{
    public class Route
    {
        private static Section _defaultSection = new Section
        {
            Name = "Default"
        };
        
        public List<WayPoint> Waypoints { get; set; }
        
        public List<Section> Sections { get; set; }

        public Section GetWaypointSection(WayPoint waypoint)
        {
            return Sections?
                .FirstOrDefault(section => section.IsPointInSection(waypoint)) ?? _defaultSection;
        }
        
        public IEnumerable<WayPoint> GetEnumerable(WayPoint start, Direction direction)
        {
            return direction == Direction.Forward ? GetForwardEnumerable(start) : GetBackwardEnumerable(start);
        }

        private IEnumerable<WayPoint> GetForwardEnumerable(WayPoint start)
        {
            var startIndex = Waypoints.IndexOf(start);
            var index = startIndex;

            do
            {
                yield return Waypoints[index];
                if (++index == Waypoints.Count) index = 0;
            } while (index != startIndex);
        }
        
        private IEnumerable<WayPoint> GetBackwardEnumerable(WayPoint start)
        {
            var startIndex = Waypoints.IndexOf(start);
            var index = startIndex;

            do
            {
                yield return Waypoints[index];
                if (--index < 0) index = Waypoints.Count - 1;
            } while (index != startIndex);
        }

        public WayPoint FindClosestWaypoint(Position position)
        {
            return Waypoints
                .OrderBy(waypoint => waypoint.CalculateDistance(position))
                .First();
        }
        
        public Direction DetermineLongestDirectionInSection(WayPoint startingWaypoint, Section currentSection)
        {
            return new[] {Direction.Forward, Direction.Backward}
                .OrderByDescending(direction =>
                {
                    var wayPoints = GetEnumerable(startingWaypoint, direction)
                        .TakeWhile(waypoint => GetWaypointSection(waypoint) == currentSection)
                        .ToArray();
                    
                    double distance = 0;

                    if (wayPoints.Length == 1) return distance;
                    
                    for (var index = 0; index < wayPoints.Length - 1; index++)
                    {
                        var wayPoint = wayPoints[index];
                        var nextWaypoint = wayPoints[index + 1];

                        distance += wayPoint.CalculateDistance(nextWaypoint);
                    }

                    return distance;
                })
                .First();
        }
    }
}
