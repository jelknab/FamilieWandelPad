using System.Collections.Generic;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Navigation.Route.waypoints;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Navigation.Route
{
    public class Section
    {
        public string Name { get; set; }
        
        public List<Position> Polygon { get; set; }

        public bool IsPointInSection(Position point)
        {
            var pointInPolygon = false;

            var x = point.Longitude;
            var y = point.Latitude;

            var j = Polygon.Count - 1;
            for (var i = 0; i < Polygon.Count; i++)
            {
                if (Polygon[i].Latitude > y != Polygon[j].Latitude > y
                    && x < Polygon[i].Longitude + (Polygon[j].Longitude - Polygon[i].Longitude) *
                    (y - Polygon[i].Latitude) / (Polygon[j].Latitude - Polygon[i].Latitude))
                {
                    pointInPolygon = !pointInPolygon;
                }

                j = i;
            }

            return pointInPolygon;
        }
    }
}