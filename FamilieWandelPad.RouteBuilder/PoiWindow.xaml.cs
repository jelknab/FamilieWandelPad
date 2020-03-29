using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.RouteBuilder.Editing;
using Microsoft.Win32;

namespace FamilieWandelPad.RouteBuilder
{
    public partial class PoiWindow : Window
    {
        private readonly CultureInfo[] _cultures =
        {
            new CultureInfo("nl"),
            new CultureInfo("en"),
            new CultureInfo("es")
        };

        private readonly IRouteController _route;
        private readonly PointOfInterest _routePoint;

        public PoiWindow(IRouteController route, PointOfInterest routePoint)
        {
            _route = route;
            _routePoint = routePoint;
            InitializeComponent();

            foreach (var culture in _cultures)
            {
                var translation = routePoint.Translations?.FirstOrDefault(t => t.Language == culture.Name)
                                  ??
                                  new Translation {Language = culture.Name};

                if (routePoint.Translations == null) routePoint.Translations = new List<Translation>();
                if (!routePoint.Translations.Contains(translation))
                    routePoint.Translations.Add(translation);
                
                var tab = new TabItem();
                var textField = new TextBox();
                var binding = new Binding("TextProperty")
                {
                    Source = translation,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                textField.SetBinding(TextBox.TextProperty, binding);

                tab.Header = culture.NativeName;
                tab.Content = textField;
                
                Image.Source = LoadImage(_routePoint.Image);

                Tabs.Items.Add(tab);
            }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _routePoint.Image = File.ReadAllBytes(openFileDialog.FileName);
                Image.Source = LoadImage(_routePoint.Image);
            }
        }


        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }

            image.Freeze();
            return image;
        }
    }
}