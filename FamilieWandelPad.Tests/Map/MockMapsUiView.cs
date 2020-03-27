using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Map;
using Mapsui.Layers;

namespace FamilieWandelPad.Tests.Map
{
    public class MockMapsUiView : INavigationMap
    {
        public void CenterView(GeoPosition position, double rotation)
        {
        }

        public void AddLayer(ILayer layer)
        {
        }
    }
}