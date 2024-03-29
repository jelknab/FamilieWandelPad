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
    public partial class MenuPage : ContentPage
    {
        private readonly INavigator _navigator;

        public MenuPage(INavigator navigator)
        {
            _navigator = navigator;
            InitializeComponent();
        }

        private void AboutButtonClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
        }

        private void ShutdownButtonClicked(object sender, EventArgs e)
        {
            var closer = DependencyService.Get<ICloseApplication>();
            closer?.closeApplication();
        }

        private void SkipButtonClicked(object sender, EventArgs e)
        {
            _navigator.SkipToCurrentLocation();
            Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}