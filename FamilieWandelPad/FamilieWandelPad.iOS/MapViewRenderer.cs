using CoreGraphics;
using FamilieWandelPad;
using FamilieWandelPad.Map;
using Foundation;
using Mapsui.Forms.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MapsUiView), typeof(MapViewRenderer))]
namespace FamilieWandelPad.iOS
{
    [Preserve(AllMembers = true)]
    public class MapViewRenderer : ViewRenderer<MapsUiView, Mapsui.UI.iOS.MapControl>
    {
        private Mapsui.UI.iOS.MapControl _mapNativeControl;
        private MapsUiView _mapViewControl;

        protected override void OnElementChanged(ElementChangedEventArgs<MapsUiView> e)
        {
            base.OnElementChanged(e);

            if (_mapViewControl == null && e.NewElement != null)
                _mapViewControl = e.NewElement;

            if (_mapNativeControl != null || _mapViewControl == null) return;
            
            var (x, y, width, height) = _mapViewControl.Bounds;
            var rect = new CGRect(x, y, width, height);

            _mapNativeControl = new Mapsui.UI.iOS.MapControl(rect);
            _mapNativeControl.Map = _mapViewControl.NativeMap;
            _mapNativeControl.Frame = rect;

            SetNativeControl(_mapNativeControl);
        }
    }
}