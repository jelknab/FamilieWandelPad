using System.Collections.Generic;
using System.Linq;

namespace FamilieWandelPad.Database.Model
{
    public class Section
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public List<GeoPosition> Polygon { get; set; }

        public bool IsPointInSection(GeoPosition point)
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
                    pointInPolygon = !pointInPolygon;

                j = i;
            }

            return pointInPolygon;
        }

        public void PrepareForSaving()
        {
            Id = 0;
        
            Polygon.ForEach(point =>
            {
                point.OrderIndex = Polygon.IndexOf(point);
                point.Id = 0;
            });
        }

        public void RecoverFromLoading()
        {
            Polygon = Polygon.OrderBy(position => position.OrderIndex).ToList();
        }
    }
}