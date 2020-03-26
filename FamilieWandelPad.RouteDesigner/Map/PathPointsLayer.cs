using System.Collections.Generic;
using System.Linq;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteDesigner.Map
{
    public class PathPointsLayer : MemoryLayer
    {
        private MemoryProvider _memoryProvider { get; set; }

        public PathPointsLayer()
        {
            _memoryProvider = new MemoryProvider();
            DataSource = _memoryProvider;
            IsMapInfoLayer = true;
            Style = null;
        }

        public void Update(List<Point> points)
        {
            _memoryProvider.Clear();
            _memoryProvider.ReplaceFeatures(
                points
                    .Select(point => new PathPointFeature()
                    {
                        Geometry = point
                    })
            );

            DataHasChanged();
        }
    }
}