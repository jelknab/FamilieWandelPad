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
        public async Task CheckAndAsk()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            switch (status)
            {
                case PermissionStatus.Granted:
                case PermissionStatus.Restricted:
                    return;
                case PermissionStatus.Unknown:
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    await CheckAndAsk();
                    break;
                case PermissionStatus.Denied:
                    ShowToSettingsPopup();
                    break;
            }
        }

        public void ShowToSettingsPopup()
        {
            var okAlertController = UIAlertController.Create (AppResources.LocationPermission, AppResources.EnableLocationFromSettings, UIAlertControllerStyle.Alert);

            //Add Action
            okAlertController.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, action =>
            {
                UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
            }));

            // Present Alert
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(okAlertController, true, null);
        }
    }
}