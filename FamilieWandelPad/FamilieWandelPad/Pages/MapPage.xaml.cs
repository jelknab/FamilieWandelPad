using System.Linq;
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
    public partial class MainPage
    {
        public Route Route { get; set; }
        
        public MainPage()
        {
            InitializeComponent();
            Route = RouteRepository.GetRoute(App.RouteFile);

            var map = MapControl.Map;

            map.Layers.Add(GetKaagTileLayer());

            map.Layers.Add(
                new PathLayer(
                    Route.GetEnumerable(Route.Waypoints.First(), Direction.Forward),
                    Consts.MainPathLayerName
                )
            );
        }

        public Navigator Navigator { get; set; }

        protected override async void OnAppearing()
        {
            if (Navigator != null) return;
            Navigator = new Navigator(MapControl, Route, CrossGeolocator.Current);

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