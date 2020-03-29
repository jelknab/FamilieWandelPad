using System.Linq;
using FamilieWandelPad.Database.Model;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteBuilder.Map
{
    public class RouteLayer : MemoryLayer
    {
        public RouteLayer()
        {
            Feature = new Feature
            {
                Geometry = new LineString()
            };
            DataSource = new MemoryProvider(Feature);
            Style = new VectorStyle
            {
                Enabled = true,
                Line = new Pen(Color.Red)
                {
                    PenStyle = PenStyle.Solid,
                    Width = 3d
                }
            };
        }

        public Feature Feature { get; set; }

        public void UpdatePath(Route route)
        {
            Feature.RenderedGeometry.Clear();
            Feature.Geometry = RenderPath(route);

            DataHasChanged();
        }

        private LineString RenderPath(Route route)
        {
            return new LineString
            {
                Vertices = route.Waypoints
                    .Select(wp => SphericalMercator.FromLonLat(wp.Longitude, wp.Latitude))
                    .ToList()
            };
        }
    }
}