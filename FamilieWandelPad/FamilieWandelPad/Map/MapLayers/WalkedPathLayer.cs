using System.Collections.Generic;
using System.Linq;
using Mapsui.Geometries;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Map.MapLayers
{
    public class WalkedPathLayer : PathLayer
    {
        public Position ExpectedPosition { get; set; }
        
        public WalkedPathLayer(IEnumerable<Position> path, string name) : base(path, name)
        {
            ExpectedPosition = Path.First();
            DataSource = new MemoryProvider(GetFeatures());
            Style = new VectorStyle
            {
                Enabled = true,
                Line = new Pen(Color.Green)
                {
                    PenStyle = PenStyle.Solid,
                    Width = 3d
                }
            }; 
        }

        public void UpdatePath(Position expectedPosition)
        {
            ExpectedPosition = expectedPosition;
            base.UpdatePath();
        }
        
        public new LineString GetFeatures()
        {
            return new LineString
            {
                Vertices = Path
                    .Select(position => SphericalMercator.FromLonLat(position.Longitude, position.Latitude))
                    .Append(SphericalMercator.FromLonLat(ExpectedPosition.Longitude, ExpectedPosition.Latitude))
                    .ToList()
            };
        }
    }
}