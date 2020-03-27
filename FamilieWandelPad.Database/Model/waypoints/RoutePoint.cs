namespace FamilieWandelPad.Database.Model.waypoints
{
    public abstract class RoutePoint : GeoPosition
    {
        protected RoutePoint(double latitude, double longitude) : base(latitude, longitude)
        {
        }

        protected RoutePoint()
        {
            
        }
    }
}