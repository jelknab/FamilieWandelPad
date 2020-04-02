using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Map;
using FamilieWandelPad.navigation;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Tests.Navigation
{
    public class NavigatorMockSpy : Navigator
    {
        public bool ShowPOICalled = false;
        public bool NavigationFinishedCalled = false;
        
        public NavigatorMockSpy(INavigationMap mapView, Database.Model.Route route, IGeolocator geoLocator,
            NavigationStats navigationStats) : base(mapView, route, geoLocator, navigationStats)
        {
        }


        protected override void ShowPointOfInterestModal(PointOfInterest poi)
        {
            ShowPOICalled = true;
        }

        public override void OnNavigationFinished()
        {
            NavigationFinishedCalled = true;
        }
    }
}