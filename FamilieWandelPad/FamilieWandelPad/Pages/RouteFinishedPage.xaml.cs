using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilieWandelPad.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RouteFinishedPage : ContentPage
    {
        public RouteFinishedPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync("http://kaag.nl");
        }
    }
}