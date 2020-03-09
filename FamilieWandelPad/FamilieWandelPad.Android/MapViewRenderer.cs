

using Android.Content;
using FamilieWandelPad;
using FamilieWandelPad.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(MapsUIView), typeof(MapViewRenderer))]
namespace FamilieWandelPad.Droid
{
    public class MapViewRenderer : ViewRenderer<MapsUIView, Mapsui.UI.Android.MapControl>
    {
        private Mapsui.UI.Android.MapControl _mapNativeControl;
        private MapsUIView _mapViewControl;

        public MapViewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapsUIView> e)
        {
            base.OnElementChanged(e);

            if (_mapViewControl == null && e.NewElement != null)
                _mapViewControl = e.NewElement;

            if (_mapNativeControl != null || _mapViewControl == null) return;
            
            _mapNativeControl = new Mapsui.UI.Android.MapControl(Context, null) {Map = _mapViewControl.NativeMap};

            SetNativeControl(_mapNativeControl);
        }
    }
}