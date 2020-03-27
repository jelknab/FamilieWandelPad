using System.Collections.Generic;
using System.Linq;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms.Internals;

namespace FamilieWandelPad.navigation
{
    public static class RouteExtensions
    {
        private static readonly Section _defaultSection = new Section
        {
            Name = "Default"
        };

        public static Direction DetermineLongestDirectionInSection(this Route route, RoutePoint startingWaypoint,
            Section section)
        {
            var directions = new[] {Direction.Forward, Direction.Backward};

            return directions
                .OrderByDescending(direction =>
                {
                    var wayPoints = route.GetEnumerable(startingWaypoint, direction)
                        .TakeWhile(waypoint => route.GetWaypointSection(waypoint) == section)
                        .ToArray();

                    double distance = 0;

                    if (wayPoints.Length == 1) return distance;

                    for (var index = 0; index < wayPoints.Length - 1; index++)
                    {
                        var wayPoint = wayPoints[index];
                        var nextWaypoint = wayPoints[index + 1];

                        distance += wayPoint.Distance(nextWaypoint);
                    }

                    return distance;
                })
                .ThenBy(direction => directions.IndexOf(direction))
                .First();
        }

        public static Section GetWaypointSection(this Route route, RoutePoint waypoint)
        {
            return route.Sections?
                .FirstOrDefault(section => section.IsPointInSection(waypoint)) ?? _defaultSection;
        }

        public static RoutePoint FindClosestWaypoint(this Route route, GeoPosition geoPosition)
        {
            return route.Waypoints
                .OrderBy(waypoint => waypoint.Distance(geoPosition))
                .First();
        }

        public static double Distance(this GeoPosition a, GeoPosition b)
        {
            return GeolocatorUtils.CalculateDistance(a.Latitude, a.Longitude, b.Latitude, b.Longitude);
        }

        public static IEnumerable<RoutePoint> GetEnumerable(this Route route, RoutePoint start, Direction direction)
        {
            return direction == Direction.Forward
                ? GetForwardEnumerable(route, start)
                : GetBackwardEnumerable(route, start);
        }

        private static IEnumerable<RoutePoint> GetForwardEnumerable(this Route route, RoutePoint start)
        {
            var startIndex = route.Waypoints.IndexOf(start);
            var index = startIndex;

            do
            {
                yield return route.Waypoints[index];
                if (++index == route.Waypoints.Count) index = 0;
            } while (index != startIndex);

            yield return start;
        }

        private static IEnumerable<RoutePoint> GetBackwardEnumerable(this Route route, RoutePoint start)
        {
            var startIndex = route.Waypoints.IndexOf(start);
            var index = startIndex;

            do
            {
                yield return route.Waypoints[index];
                if (--index < 0) index = route.Waypoints.Count - 1;
            } while (index != startIndex);

            yield return start;
        }
    }
}