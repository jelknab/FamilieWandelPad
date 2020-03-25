using System;
using Mapsui;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Styles;
using Mapsui.UI.Forms;
using Position = Plugin.Geolocator.Abstractions.Position;

namespace FamilieWandelPad.Map
{
    public class MapsUiView : MapView, INavigationMap
    {
        public MapsUiView()
        {
            Map = new Mapsui.Map
            {
                BackColor = Color.White,
                Home = n => n.NavigateTo(SphericalMercator.FromLonLat(52.22002, 4.55835), 0.6)
            };
            
            Navigator = new AnimatedNavigatorRot(Map, (IViewport) Viewport);
        }
        
        public void CenterView(Position position, double rotation)
        {
            ((AnimatedNavigatorRot) Navigator).NavigateTo(position.ToMapSui(), 0.6, 0, 1000L);
        }

        public void AddLayer(ILayer layer)
        {
            Map.Layers.Add(layer);
        }
    }
}
