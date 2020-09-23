using System.Threading.Tasks;
using FamilieWandelPad.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationPermission))]
namespace FamilieWandelPad.Droid
{
    public class LocationPermission : ILocationPermission
    {
        public async Task<bool> CheckAndAsk()
        {
            if (await IsLocationGranted()) return true;
            
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (await IsLocationGranted()) return true;
            
            var closer = DependencyService.Get<ICloseApplication>();
            closer?.closeApplication();

            return false;
        }
        
        private static async Task<bool> IsLocationGranted()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            return status == PermissionStatus.Granted || status == PermissionStatus.Restricted;
        }
    }
}