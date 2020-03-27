namespace FamilieWandelPad.Database.Model.waypoints
{
    public class WayPoint : RoutePoint
    {
        public WayPoint()
        {
            
        }
        
        public WayPoint(GeoPosition routePoint) : base(routePoint.Latitude, routePoint.Longitude)
        {
        }

        public WayPoint(double latitude, double longitude) : base(latitude, longitude)
        {
        }
    }
}