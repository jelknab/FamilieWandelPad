using FamilieWandelPad.Pages;
using Plugin.Geolocator;
using SQLite;
using Xamarin.Forms;

namespace FamilieWandelPad
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!CrossGeolocator.IsSupported)
            {
                MainPage = new UnsupportedPage();
                return;
            }

            MainPage = new WelcomePage();
        }

        public static SQLiteConnectionString MbTileConnectionString { get; set; }
        public static string RouteFile { get; set; }

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