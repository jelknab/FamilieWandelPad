using System.Linq;
using System.Threading.Tasks;
using BruTile.MbTiles;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Repositories;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.navigation;
using Mapsui.Layers;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace FamilieWandelPad.Pages
{
    public partial class MapPage
    {
        private readonly Task<Route> _routeTask;
        
        public MapPage(Task<Route> routeTask)
        {
            _routeTask = routeTask;
            InitializeComponent();

            MapControl.Map.Layers.Add(GetKaagTileLayer());
        }

        public Navigator Navigator { get; set; }

        protected override async void OnAppearing()
        {
            if (Navigator != null) return;

            var route = await _routeTask;
            MapControl.Map.Layers.Add(
                new PathLayer(
                    route.GetEnumerable(route.Waypoints.First(), Direction.Forward),
                    Consts.MainPathLayerName
                )
            );
            Navigator = new Navigator(MapControl, route, CrossGeolocator.Current);

            await Application.Current.MainPage.Navigation.PushModalAsync(new LookingForGpsPage());
            await Navigator.StartNavigation();
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private static TileLayer GetKaagTileLayer()
        {
            var mbTilesTileSource = new MbTilesTileSource(App.MbTileConnectionString);
            var mbTilesLayer = new TileLayer(mbTilesTileSource);
            return mbTilesLayer;
        }
    }
}