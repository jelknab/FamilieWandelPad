using FamilieWandelPad.Map.MapLayers;
using Mapsui.Geometries;
using Mapsui.Layers;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Map
{
    public interface INavigationMap
    {
        void CenterView(Position position, double rotation);
        
        void AddLayer(ILayer layer);
    }
}