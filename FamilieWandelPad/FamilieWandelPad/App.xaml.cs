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

            MainPage = new NavigationPage(new MainPage());
        }

        public static SQLiteConnectionString MbTileConnectionString { get; set; }

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