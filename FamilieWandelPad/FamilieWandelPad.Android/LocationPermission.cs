using System.Threading.Tasks;
using FamilieWandelPad.Droid;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationPermission))]
namespace FamilieWandelPad.Droid
{
    public class LocationPermission : ILocationPermission
    {
        public async Task CheckAndAsk()
        {
            if (await IsLocationGranted()) return;
            
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (await IsLocationGranted()) return;
            
            var closer = DependencyService.Get<ICloseApplication>();
            closer?.closeApplication();
        }
        
        private static async Task<bool> IsLocationGranted()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            return status == PermissionStatus.Granted || status == PermissionStatus.Restricted;
        }
    }
}