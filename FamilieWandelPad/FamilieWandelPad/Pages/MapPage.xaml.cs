using BruTile.MbTiles;
using FamilieWandelPad.navigation;
using Mapsui.Layers;
using Xamarin.Forms;

namespace FamilieWandelPad.Pages
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            var map = MapControl.Map;

            map.Layers.Add(GetKaagTileLayer());
            // map.Layers.Add(
            //     new PathLayer(
            //         DebugRoute.GetEnumerable(DebugRoute.Waypoints.First(), Direction.Forward),
            //         Consts.MainPathLayerName
            //     )
            // );
        }

        public Navigator Navigator { get; set; }

        protected override async void OnAppearing()
        {
            if (Navigator != null) return;
            // Navigator = new Navigator(MapControl, DebugRoute, CrossGeolocator.Current);

            await Application.Current.MainPage.Navigation.PushModalAsync(new LookingForGpsPage());
            await Navigator.StartNavigation();
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public static TileLayer GetKaagTileLayer()
        {
            var mbTilesTileSource = new MbTilesTileSource(App.MbTileConnectionString);
            var mbTilesLayer = new TileLayer(mbTilesTileSource);
            return mbTilesLayer;
        }
    }
}