using System.Collections.Generic;
using FamilieWandelPad.Database.Model;
using Mapsui.Styles;

namespace FamilieWandelPad.Map.MapLayers
{
    public class WalkedPathLayer : PathLayer
    {
        public WalkedPathLayer(IEnumerable<GeoPosition> path, string name) : base(path, name)
        {
            Style = new VectorStyle
            {
                Enabled = true,
                Line = new Pen(Color.FromArgb(255, 120, 120, 120))
                {
                    PenStyle = PenStyle.Solid,
                    Width = 12d
                }
            };
        }
    }
}