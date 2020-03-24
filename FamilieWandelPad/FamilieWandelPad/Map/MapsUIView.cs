using Mapsui.Layers;
using Mapsui.Styles;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Map
{
    public class MapsUiView : Xamarin.Forms.View, INavigationMap
    {
        public Mapsui.Map NativeMap { get; }
 
        public MapsUiView()
        {
            NativeMap = new Mapsui.Map {BackColor = Color.White};
        }

        public void Update()
        {
            NativeMap.ViewChanged(true);
        }

        public void CenterView(Position position)
        {
            NativeMap.NavigateTo(position.ToMapSui());
        }

        public void AddLayer(ILayer layer)
        {
            NativeMap.Layers.Add(layer);
        }
    }
}
