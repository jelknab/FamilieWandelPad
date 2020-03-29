using System.Linq;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.RouteBuilder.Map.Features;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteBuilder.Map
{
    public class SectionPointsLayer : MemoryLayer
    {
        public SectionPointsLayer()
        {
            _memoryProvider = new MemoryProvider();
            DataSource = _memoryProvider;
            IsMapInfoLayer = true;
            Style = new SymbolStyle
            {
                Enabled = true,
                SymbolType = SymbolType.Ellipse,
                SymbolScale = 0.25,
                Fill = new Brush(new Color(40, 40, 40))
            };
        }

        private MemoryProvider _memoryProvider { get; }

        public void Update(Route route)
        {
            _memoryProvider.Clear();
            _memoryProvider.ReplaceFeatures(
                route.Sections
                    .SelectMany(section => section.Polygon)
                    .Select(polygonPoint => new SectionPointFeature
                    {
                        Section = route.Sections.Find(section => section.Polygon.Contains(polygonPoint)),
                        Point = polygonPoint,
                        Geometry = SphericalMercator.FromLonLat(polygonPoint.Longitude, polygonPoint.Latitude)
                    })
            );

            DataHasChanged();
        }
    }
}