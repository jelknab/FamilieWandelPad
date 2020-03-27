using System.Collections.Generic;

namespace FamilieWandelPad.Database.Model.waypoints
{
    public class PointOfInterest : RoutePoint
    {
        public PointOfInterest() {}
        
        public PointOfInterest(GeoPosition wp) : base(wp.Latitude, wp.Longitude)
        {
        }

        public PointOfInterest(double latitude, double longitude) : base(latitude, longitude)
        {
        }

        public List<Translation> Translations { get; set; }
        
        public byte[] Image { get; set; }
    }
}