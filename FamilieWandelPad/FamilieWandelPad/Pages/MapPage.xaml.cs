using System.Collections.Generic;
using System.Linq;
using BruTile.MbTiles;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.Navigation.Route;
using FamilieWandelPad.Navigation.Route.waypoints;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Widgets;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets.Zoom;
using Plugin.Geolocator;
using Xamarin.Forms;
using Navigator = FamilieWandelPad.navigation.Navigator;

namespace FamilieWandelPad.Pages
{
    public partial class MainPage
    {
        private static readonly Route DebugRoute = new Route(
            waypoints: new List<WayPoint>()
            {
                new WayPoint(52.22002, 4.55835),
                new WayPoint(52.21937, 4.55812),
                new PointOfInterest(52.21944, 4.55774) {Description = "debug"},
                new WayPoint(52.21975, 4.55711),
                new WayPoint(52.21985, 4.55682),
            },
            sections: null
        );

        public Navigator Navigator { get; set; }

        public MainPage()
        {
            InitializeComponent();

            var map = MapControl.Map;

            map.Layers.Add(GetKaagTileLayer());
            map.Layers.Add(
                new PathLayer(
                    DebugRoute.Waypoints,
                    Consts.MainPathLayerName
                )
            );
        }

        protected override async void OnAppearing()
        {
            if (Navigator != null) return;
            Navigator = new Navigator(MapControl, DebugRoute, CrossGeolocator.Current);

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