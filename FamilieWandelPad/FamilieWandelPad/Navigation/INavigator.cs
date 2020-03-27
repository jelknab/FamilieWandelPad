using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.navigation
{
    public interface INavigator
    {
        Task StartNavigation();

        void OnNavigationFinished();

        void OnLocationUpdate(object sender, PositionEventArgs e);
    }
}