using System.Collections.Generic;
using System.Linq;
using FamilieWandelPad.Database.Model;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.Map.MapLayers
{
    public class PathLayer : MemoryLayer
    {
        public PathLayer(IEnumerable<GeoPosition> path, string name)
        {
            feature = new Feature
            {
                Geometry = RenderPath(path)
            };
            Name = name;
            DataSource = new MemoryProvider(feature);
            Style = new VectorStyle
            {
                Enabled = true,
                Line = new Pen(Color.FromArgb(255, 75, 75, 150))
                {
                    PenStyle = PenStyle.Solid,
                    Width = 3d
                }
            };
        }

        public Feature feature { get; set; }

        public void UpdatePath(IEnumerable<GeoPosition> path)
        {
            feature.RenderedGeometry.Clear();
            feature.Geometry = RenderPath(path);

            DataHasChanged();
        }

        private LineString RenderPath(IEnumerable<GeoPosition> path)
        {
            return new LineString
            {
                Vertices = path
                    .Select(position => position.ToMapSui())
                    .ToList()
            };
        }
    }
}