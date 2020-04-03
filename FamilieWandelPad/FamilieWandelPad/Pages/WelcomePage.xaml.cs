using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Repositories;
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var navigationPage = new NavigationPage(new MapPage(RouteTask))
            {
                BarBackgroundColor = Color.CornflowerBlue
            };

            Application.Current.MainPage = navigationPage;
        }
    }
}