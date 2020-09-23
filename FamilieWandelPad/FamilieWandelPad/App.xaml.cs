using System;
using System.Globalization;
using FamilieWandelPad.Pages;
using FamilieWandelPad.Resx;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Geolocator;
using SQLite;
using Xamarin.Forms;

namespace FamilieWandelPad
{
    public partial class App : Application
    {
        public App()
        {
            AppResources.Culture = CultureInfo.CurrentUICulture;
            
            InitializeComponent();

            if (!CrossGeolocator.IsSupported)
            {
                MainPage = new UnsupportedPage();
                return;
            }
            
            var fileHelper = DependencyService.Get<IFileAccessHelper>();
            MbTileConnectionString = new SQLiteConnectionString(fileHelper.MakeAssetAvailable("Kaag.mbtiles"));
            RouteFile = fileHelper.MakeAssetAvailable("route.sqlite");

            MainPage = new WelcomePage();
        }

        public static SQLiteConnectionString MbTileConnectionString { get; set; }
        public static string RouteFile { get; set; }

        protected override void OnStart()
        {
            
            AppCenter.Start("ios=de6d78ad-6088-4dad-ba00-a6f5188e028e;",
                   typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}