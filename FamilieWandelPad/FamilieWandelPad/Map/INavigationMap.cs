using FamilieWandelPad.Database.Model;
using Mapsui.Layers;

namespace FamilieWandelPad.Map
{
    public interface INavigationMap
    {
        void CenterView(GeoPosition position, double rotation);

        void AddLayer(ILayer layer);
    }
}