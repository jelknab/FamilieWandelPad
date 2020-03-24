using FamilieWandelPad.Map.MapLayers;
using Mapsui.Layers;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Map
{
    public interface INavigationMap
    {
        void Update();
        
        void CenterView(Position position);
        void AddLayer(ILayer layer);
    }
}