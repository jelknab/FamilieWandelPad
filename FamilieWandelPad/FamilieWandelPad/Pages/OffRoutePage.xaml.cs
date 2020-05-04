using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilieWandelPad.navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilieWandelPad.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OffRoutePage : ContentPage
    {
        private readonly INavigator _navigator;

        public OffRoutePage(INavigator navigator)
        {
            _navigator = navigator;
            InitializeComponent();
        }


        private void RouteReset_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new WelcomePage();
        }

        private void RouteSkip_OnClicked(object sender, EventArgs e)
        {
            _navigator.SkipToCurrentLocation();
            Application.Current.MainPage.Navigation.PopAsync();
        }

        private void Ignore_OnClicked(object sender, EventArgs e)
        {
            _navigator.StopOffRoutePopup();
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}