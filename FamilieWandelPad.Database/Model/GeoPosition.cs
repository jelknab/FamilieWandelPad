namespace FamilieWandelPad.Database.Model
{
    public class GeoPosition
    {
        public int Id { get; set; }

        public int OrderIndex { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
        
        public GeoPosition()
        {
        }

        public GeoPosition(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}