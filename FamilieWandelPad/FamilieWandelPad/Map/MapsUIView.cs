using System;
using System.Threading;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.UI.Forms;

namespace FamilieWandelPad.Map
{
    public class MapsUiView : MapView, INavigationMap
    {
        private bool _isPanning { get; set; } = false;
        
        public MapsUiView()
        {
            Map = new Mapsui.Map
            {
                BackColor = Color.White,
                Home = n => n.NavigateTo(SphericalMercator.FromLonLat(52.22002, 4.55835), 0.2)
            };
            IsMyLocationButtonVisible = false;
            IsNorthingButtonVisible = false;
            IsZoomButtonVisible = false;
            MyLocationEnabled = false;

            CancellationTokenSource cancelToken = null;
            
            TouchStarted += (sender, args) => _isPanning = true;
            TouchEnded += async (sender, args) =>
            {
                cancelToken?.Cancel();
                cancelToken = new CancellationTokenSource();
                
                try {
                    await Task.Delay(2000, cancelToken.Token);
                    _isPanning = false;
                }
                catch (OperationCanceledException) {}
            };

            Navigator = new AnimatedNavigatorWithRotation(Map, (IViewport) Viewport);
        }

        public void CenterView(GeoPosition position, double rotation)
        {
            if (_isPanning) return;
            
            ((AnimatedNavigatorWithRotation) Navigator)
                .NavigateTo(position.ToMapSui(), Viewport.Resolution, rotation, 1000L);
        }

        public void AddLayer(ILayer layer)
        {
            Map.Layers.Add(layer);
        }
    }
}