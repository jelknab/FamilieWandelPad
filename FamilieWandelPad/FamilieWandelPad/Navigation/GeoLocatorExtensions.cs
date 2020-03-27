using FamilieWandelPad.Database.Model;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.navigation
{
    public static class GeoLocatorExtensions
    {
        public static GeoPosition ToGeoPosition(this Position position)
        {
            return new GeoPosition
            {
                Latitude = position.Latitude,
                Longitude = position.Longitude
            };
        }

        public static Position ToGeoLocatorPosition(this GeoPosition geoPosition)
        {
            return new Position(geoPosition.Latitude, geoPosition.Longitude);
        }
    }
}