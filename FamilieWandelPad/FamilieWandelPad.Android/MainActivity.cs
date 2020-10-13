using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.Widget;
using FamilieWandelPad.Database.Repositories;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

namespace FamilieWandelPad.Droid
{
    [Activity(Label = "Familie Wandelpad", Icon = "@mipmap/icon", Theme = "@style/MainTheme",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private static bool _falseFlag = false;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            CrossCurrentActivity.Current.Activity = this;


            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            
            LinkerFix();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void LinkerFix()
        {
            if (_falseFlag)
            {
                var ignore = new FitWindowsLinearLayout(null);
                // RouteRepository.GetRouteAsync(null);
            }
        }
    }
}