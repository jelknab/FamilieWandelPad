using FamilieWandelPad.navigation;

namespace FamilieWandelPad.Navigation.Route.waypoints
{
    public class PointOfInterest : WayPoint
    {
        public PointOfInterest(float latitude, float longitude) : base(latitude, longitude)
        {
        }
        
        public string Description { get; set; }

        public new void OnArrival(INavigator navigator)
        {
            base.OnArrival(navigator);

            //Todo: logic for showing information
        }

        
    }
}
