using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilieWandelPad.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PointOfInterestPage : ContentPage
    {
        public PointOfInterestPage(PointOfInterest pointOfInterest)
        {
            InitializeComponent();
            var r = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            var translation = pointOfInterest.Translations.FirstOrDefault(t => t.Language == r)
                              ??
                              pointOfInterest.Translations.FirstOrDefault(t => t.Language == "en")
                ?? new Translation{Text = "Translation missing :("};

            WebView.Source = new HtmlWebViewSource
            {
                Html = translation.Text
            };
            
            WebView.Navigating += (s, e) =>
            {
                if (e.Url.StartsWith("http"))
                {
                    try
                    {
                        var uri = new Uri(e.Url);
                        Launcher.OpenAsync(uri);
                    }
                    catch (Exception)
                    {
                    }

                    e.Cancel = true;
                }
            };

            if (pointOfInterest.Image != null)
            {
                HeaderImage.Source = ImageSource.FromStream(() => new MemoryStream(pointOfInterest.Image));
                var tap = new TapGestureRecognizer();
                tap.Tapped += (sender, args) =>
                {
                    var fullScreenImage = new FullScreenImageModal(pointOfInterest.Image);
                    Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(fullScreenImage));
                };
                HeaderImage.GestureRecognizers.Add(tap);
            }
        }
    }
}