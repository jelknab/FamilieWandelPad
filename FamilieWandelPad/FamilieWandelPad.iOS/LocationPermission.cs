using System.Threading.Tasks;
using FamilieWandelPad.iOS;
using FamilieWandelPad.Resx;
using Foundation;
using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationPermission))]
namespace FamilieWandelPad.iOS
{
    public class LocationPermission : ILocationPermission
    {
        public async Task<bool> CheckAndAsk()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            switch (status)
            {
                case PermissionStatus.Granted:
                    return true;
                case PermissionStatus.Restricted:
                    return false;
                case PermissionStatus.Unknown:
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    return await CheckAndAsk();
                case PermissionStatus.Denied:
                    ShowToSettingsPopup();
                    return false;
                default:
                    return false;
            }
        }

        public void ShowToSettingsPopup()
        {
            var okAlertController = UIAlertController.Create (AppResources.LocationPermission, AppResources.EnableLocationFromSettings, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, action =>
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                var closer = DependencyService.Get<ICloseApplication>();
                closer?.closeApplication();
            }));

            // Present Alert
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(okAlertController, true, null);
        }
    }
}