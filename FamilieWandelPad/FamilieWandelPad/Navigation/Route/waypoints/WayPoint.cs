using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Navigation.Route.waypoints
{
    public class WayPoint : Position
    {
        public WayPoint(double latitude, double longitude) : base(latitude, longitude)
        {
        }

        public void OnArrival()
        {
            
        }
    }
}
