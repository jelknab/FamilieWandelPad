using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Map;
using FamilieWandelPad.navigation;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Tests.Navigation
{
    public class NavigatorMockSpy : Navigator
    {
        public bool ShowPOICalled = false;
        
        public NavigatorMockSpy(INavigationMap mapView, Database.Model.Route route, IGeolocator geoLocator) : base(mapView, route, geoLocator)
        {
        }

        protected override void ShowPointOfInterestModal(PointOfInterest poi)
        {
            ShowPOICalled = true;
        }
    }
}