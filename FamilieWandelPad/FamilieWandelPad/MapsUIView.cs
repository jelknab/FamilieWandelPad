using Mapsui.Styles;

namespace FamilieWandelPad
{
    public class MapsUIView : Xamarin.Forms.View
    {
        public Mapsui.Map NativeMap { get; }
 
        protected internal MapsUIView()
        {
            NativeMap = new Mapsui.Map {BackColor = Color.White};
        }
    }
}
