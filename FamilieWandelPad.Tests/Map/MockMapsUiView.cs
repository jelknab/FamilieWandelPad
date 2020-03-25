using FamilieWandelPad.Map;
using Mapsui.Layers;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Tests.Map
{
    public class MockMapsUiView : INavigationMap
    {
        
        public new void CenterView(Position position, double rotation)
        {
        }

        public new void AddLayer(ILayer layer)
        {
        }
    }
}