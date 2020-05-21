using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.navigation
{
    public interface INavigator
    {
        Task<bool> StartNavigation();

        void OnNavigationFinished();

        void OnLocationUpdate(object sender, PositionEventArgs e);
        
        Task SkipToCurrentLocation();
        
        void StopOffRoutePopup();
        
        void Stop();
    }
}