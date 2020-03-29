using System.Collections.Generic;
using System.Linq;
using FamilieWandelPad.Database.Model;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteBuilder.Map
{
    public class SectionsLayer : MemoryLayer
    {
        public List<Polygon> Polygons { get; set; }
        private MemoryProvider _memoryProvider { get; }

        public SectionsLayer()
        {
            _memoryProvider = new MemoryProvider();
            DataSource = _memoryProvider;
            Style = new VectorStyle
            {
                Enabled = true,
                Fill = new Brush(new Color(200, 50, 200, 50)),
                Outline = new Pen
                {
                    Color = Color.Orange,
                    Width = 2,
                    PenStyle = PenStyle.DashDotDot, //.Solid,
                    PenStrokeCap = PenStrokeCap.Round
                }
            };
        }

        public void UpdateSections(Route route)
        {
            _memoryProvider.Clear();
            _memoryProvider.ReplaceFeatures(RenderSections(route));

            DataHasChanged();
        }

        private IEnumerable<Feature> RenderSections(Route route)
        {
            return route.Sections
                .Select(section => new Polygon
                {
                    ExteriorRing =
                        new LinearRing(
                            section.Polygon.Select(gp => SphericalMercator.FromLonLat(gp.Longitude, gp.Latitude))
                        )
                })
                .Select(polygon => new Feature
                {
                    Geometry = polygon
                });
        }
    }
}