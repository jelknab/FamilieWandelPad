using System.Collections.Generic;
using System.IO;
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
        public Feature feature { get; set; }

        public PathLayer(IEnumerable<Position> path, string name)
        {
            feature = new Feature
            {
                Geometry = RenderPath(path)
            };
            Name = name;
            DataSource = new MemoryProvider(this.feature);
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

        public void UpdatePath(IEnumerable<Position> path)
        {
            feature.RenderedGeometry.Clear();
            feature.Geometry = RenderPath(path);
            
            DataHasChanged();
        }

        private LineString RenderPath(IEnumerable<Position> path)
        {
            return new LineString
            {
                Vertices = path
                    .Select(position => SphericalMercator.FromLonLat(position.Longitude, position.Latitude))
                    .ToList()
            };
        }
    }
}