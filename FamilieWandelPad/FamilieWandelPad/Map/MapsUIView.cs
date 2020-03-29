using FamilieWandelPad.Database.Model;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Styles;
using Mapsui.UI.Forms;

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
            MyLocationEnabled = false;
            IsMyLocationButtonVisible = false;
            IsNorthingButtonVisible = false;

            Navigator = new AnimatedNavigatorWithRotation(Map, (IViewport) Viewport);
        }

        public void CenterView(GeoPosition position, double rotation)
        {
            ((AnimatedNavigatorWithRotation) Navigator)
                .NavigateTo(position.ToMapSui(), Viewport.Resolution, rotation, 1000L);
        }

        public void AddLayer(ILayer layer)
        {
            Map.Layers.Add(layer);
        }
    }
}