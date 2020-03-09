using System.Linq;
using FamilieWandelPad.navigation;
using Mapsui.Forms.Extensions;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Xamarin.Forms.Maps;

namespace FamilieWandelPad.MapLayers
{
    public class PathLayer : MemoryLayer
    {
        private readonly Route _route;

        public PathLayer(Route route)
        {
            _route = route;
            Name = "Path";
            DataSource = new MemoryProvider(GetFeatures());
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

        public LineString GetFeatures()
        {
            return new LineString
            {
                Vertices = _route.Waypoints
                    .Select(wp => SphericalMercator.FromLonLat(wp.Longitude, wp.Latitude)).ToList()
            };
        }
    }
}