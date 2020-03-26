using System.Collections.Generic;
using System.Linq;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteDesigner.Map
{
    public class PathLayer : MemoryLayer
    {
        public Feature feature { get; set; }

        public PathLayer()
        {
            feature = new Feature
            {
                Geometry = new LineString()
            };
            DataSource = new MemoryProvider(feature);
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

        public void UpdatePath(IEnumerable<Point> path)
        {
            feature.RenderedGeometry.Clear();
            feature.Geometry = RenderPath(path);
            
            DataHasChanged();
        }

        private LineString RenderPath(IEnumerable<Point> path)
        {
            return new LineString
            {
                Vertices = path.ToList()
            };
        }
    }
}