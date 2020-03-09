using Plugin.Geolocator;
using System;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilieWandelPad
{
    public partial class App : Application
    {
        
        public static SQLiteConnectionString MbTileConnectionString { get; set; }
        
        public App()
        {
            InitializeComponent();

            MainPage = CrossGeolocator.IsSupported ? (Page)new MainPage() : (Page)new UnsupportedPage();
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
