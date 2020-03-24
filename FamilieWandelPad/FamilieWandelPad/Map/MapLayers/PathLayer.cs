using System.Collections.Generic;
using System.Linq;
using FamilieWandelPad.Navigation.Route;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Map.MapLayers
{
    public class PathLayer : MemoryLayer
    {
        protected IEnumerable<Position> Path { get; set; }

        public PathLayer(IEnumerable<Position> path, string name)
        {
            Path = path;
            Name = name;
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

        public void UpdatePath()
        {
            ClearCache();
            ViewChanged(true, Envelope, 1);
        }

        public LineString GetFeatures()
        {
            return new LineString
            {
                Vertices = Path
                    .Select(position => SphericalMercator.FromLonLat(position.Longitude, position.Latitude))
                    .ToList()
            };
        }
    }
}