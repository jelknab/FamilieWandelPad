using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Pages;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI.Forms;
using Xamarin.Forms;
using Color = Mapsui.Styles.Color;

namespace FamilieWandelPad.Map.MapLayers
{
    public class PointOfInterestLayer : MemoryLayer
    {
        public PointOfInterestLayer(IEnumerable<PointOfInterest> pointOfInterests)
        {
            DataSource = new MemoryProvider(pointOfInterests.Select(poi => new PointOfInterestFeature(poi)
            {
                Geometry = SphericalMercator.FromLonLat(poi.Longitude, poi.Latitude)
            }));
            IsMapInfoLayer = true;
            Style = CreateSvgStyle("FamilieWandelPad.Assets.pin.svg", 0.8);
        }
        
        private static SymbolStyle CreateSvgStyle(string embeddedResourcePath, double scale)
        {
            var bitmapId = GetBitmapIdForEmbeddedResource(embeddedResourcePath);
            return new SymbolStyle
            {
                BitmapId = bitmapId, 
                SymbolScale = scale, 
                SymbolOffset = new Offset(0.0, 0.5, true),
                Enabled = true
            };
        }
        
        private static int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            var assembly = typeof(PointOfInterestLayer).GetTypeInfo().Assembly;
            var image = assembly.GetManifestResourceStream(imagePath);
            var bitmapId = BitmapRegistry.Instance.Register(image);
            return bitmapId;
        }
    }

    public class PointOfInterestFeature : Feature
    {
        private readonly PointOfInterest _pointOfInterest;

        public PointOfInterestFeature(PointOfInterest pointOfInterest)
        {
            _pointOfInterest = pointOfInterest;
        }

        public async Task OnClick()
        {
            await Application.Current.MainPage.Navigation.PushAsync(
                new PointOfInterestPage(_pointOfInterest));
        }
    }
}