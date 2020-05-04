using System;
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
        public static readonly Section DefaultSection = new Section
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
                        .TakeWhile(waypoint => route.GetSectionForPosition(waypoint) == section)
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

        public static Section GetSectionForPosition(this Route route, GeoPosition position)
        {
            return route.Sections?
                .FirstOrDefault(section => section.IsPointInSection(position)) ?? DefaultSection;
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

        // https://stackoverflow.com/a/2042883/12107890
        public static double DegreeBearing(this GeoPosition a, GeoPosition b)
        {   
            var dLon = ToRad(b.Longitude-a.Longitude);
            var dPhi = Math.Log(
                Math.Tan(ToRad(b.Latitude)/2+Math.PI/4)/Math.Tan(ToRad(a.Latitude)/2+Math.PI/4));
            if (Math.Abs(dLon) > Math.PI) 
                dLon = dLon > 0 ? -(2*Math.PI-dLon) : (2*Math.PI+dLon);
            return ToBearing(Math.Atan2(dLon, dPhi));
        }

        private static double ToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        private static double ToDegrees(double radians)
        {
            return radians * 180 / Math.PI;
        }

        private static double ToBearing(double radians) 
        {  
            // convert radians to degrees (as bearing: 0...360)
            return (ToDegrees(radians) +360) % 360;
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