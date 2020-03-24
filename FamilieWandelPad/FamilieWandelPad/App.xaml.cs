using Plugin.Geolocator;
using System;
using FamilieWandelPad.Pages;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilieWandelPad
{
    public partial class App : Application
    {
        
        public static SQLiteConnectionString MbTileConnectionString { get; set; }
        public static string NavigationArrow { get; set; }

        public App()
        {
            InitializeComponent();

            if (!CrossGeolocator.IsSupported)
            {
                MainPage = new UnsupportedPage();
                return;
            }
            
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private bool IsLocationAvailable()
        {
            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

    }
}
