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
        public WalkedPathLayer(IEnumerable<Position> path, string name) : base(path, name)
        {
            Style = new VectorStyle
            {
                Enabled = true,
                Line = new Pen(Color.Gray)
                {
                    PenStyle = PenStyle.Solid,
                    Width = 3.1d
                }
            }; 
        }
    }
}