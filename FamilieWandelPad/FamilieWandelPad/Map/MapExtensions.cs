using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using FamilieWandelPad.Database.Model;
using Mapsui.Geometries;
using Mapsui.Projection;

namespace FamilieWandelPad.Map
{
    public static class MapExtensions
    {
        public static Point ToMapSui(this GeoPosition position)
        {
            return SphericalMercator.FromLonLat(position.Longitude, position.Latitude);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static GeoPosition ClosestPositionBetweenPoints(GeoPosition positionA, GeoPosition positionB,
            GeoPosition position)
        {
            var a = new Vector2((float) positionA.Latitude, (float) positionA.Longitude);
            var b = new Vector2((float) positionB.Latitude, (float) positionB.Longitude);
            var p = new Vector2((float) position.Latitude, (float) position.Longitude);

            var a_to_p = p - a; //Vector from A to P   
            var a_to_b = b - a; //Vector from A to B  

            var magnitude_a_b = a_to_b.LengthSquared(); //Magnitude of AB vector (it's length squared)     
            var a_b_a_p_product = Vector2.Dot(a_to_p, a_to_b); //The DOT product of a_to_p and a_to_b     
            var distance = a_b_a_p_product / magnitude_a_b; //The normalized "distance" from a to your closest point  

            if (distance < 0) //Check if P projection is over vectorAB     
                return positionA;

            if (distance > 1)
                return positionB;

            if (double.IsNaN(distance))
                distance = 0;

            var result = a + a_to_b * distance;
            return new GeoPosition(result.X, result.Y);
        }

        public static GeoPosition InterpolatePosition(GeoPosition positionA, GeoPosition positionB, double percent)
        {
            var percentInv = 1 - percent;

            return new GeoPosition(
                positionA.Latitude * percent + positionB.Latitude * percentInv,
                positionA.Longitude * percent + positionB.Longitude * percentInv
            );
        }

        public static double AngleTo(this GeoPosition a, GeoPosition b)
        {
            return Math.Atan2(b.Longitude - a.Longitude, b.Latitude - a.Latitude) * (180 / Math.PI);
        }
    }
}