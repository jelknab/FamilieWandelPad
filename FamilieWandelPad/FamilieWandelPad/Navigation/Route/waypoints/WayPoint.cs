using FamilieWandelPad.navigation;

namespace FamilieWandelPad.Navigation.Route.waypoints
{
    public class WayPoint : GPSCoordinate
    {
        public WayPoint(float latitude, float longitude) : base(latitude, longitude)
        {
        }

        public void OnArrival(INavigator navigator)
        {
            navigator.NavigateTo(navigator.GetNextWaypoint(this));
        }
    }
}
