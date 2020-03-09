using System;
using System.Collections.Generic;
using System.Text;
using Mapsui.Geometries;

namespace FamilieWandelPad.navigation
{
    public class GPSCoordinate
    {
        public GPSCoordinate(float latitude, float longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        
        public float Longitude { get; set; }

        public float Latitude { get; set; }
    }
}
