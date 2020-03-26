using System.Collections.Generic;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteDesigner.Map
{
    public class PathPointFeature : Feature
    {
        private static ICollection<IStyle> _styles = new List<IStyle>()
        {
            new SymbolStyle
            {
                Enabled = true,
                SymbolType = SymbolType.Ellipse,
                SymbolScale = 0.25,
                Fill = new Brush(new Color(40, 40, 40))
            }
        };

        public PathPointFeature()
        {
            Styles = _styles;
        }
    }
}