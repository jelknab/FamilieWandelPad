using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Repositories;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilieWandelPad.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        private Task<Route> RouteTask { get; set; }
        
        public WelcomePage()
        {
            InitializeComponent();
            RouteTask = RouteRepository.GetRouteAsync(App.RouteFile);
        }

        protected override async void OnAppearing()
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var navigationPage = new NavigationPage(new MapPage(RouteTask))
            {
                BarBackgroundColor = Color.CornflowerBlue,
                BarTextColor = Color.White
            };

            Application.Current.MainPage = navigationPage;
        }
    }
}