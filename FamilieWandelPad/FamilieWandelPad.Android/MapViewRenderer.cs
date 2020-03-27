using Android.Content;
using FamilieWandelPad.Droid;
using FamilieWandelPad.Map;
using Mapsui.UI.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MapsUiView), typeof(MapViewRenderer))]

namespace FamilieWandelPad.Droid
{
    public class MapViewRenderer : ViewRenderer<MapsUiView, MapControl>
    {
        private MapControl _mapNativeControl;
        private MapsUiView _mapViewControl;

        public MapViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapsUiView> e)
        {
            base.OnElementChanged(e);

            if (_mapViewControl == null && e.NewElement != null)
                _mapViewControl = e.NewElement;

            if (_mapNativeControl != null || _mapViewControl == null) return;

            _mapNativeControl = new MapControl(Context, null) {Map = _mapViewControl.Map};

            SetNativeControl(_mapNativeControl);
        }
    }
}