using System.Collections.Generic;
using BruTile.MbTiles;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Navigation.Route;
using FamilieWandelPad.Navigation.Route.waypoints;
using Mapsui.Forms.Extensions;
using Mapsui.Layers;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FamilieWandelPad.Pages
{
    public partial class MainPage
    {
        private static readonly Route DebugRoute = new Route()
        {
            Waypoints = new List<WayPoint>()
            {
                new WayPoint(52.22002f, 4.55835f),
                new WayPoint(52.21937f, 4.55812f),
                new WayPoint(52.21944f, 4.55774f),
                new WayPoint(52.21975f, 4.55711f),
                new WayPoint(52.21985f, 4.55682f),
            }
        };

        public Navigator Navigator { get; set; }

        public MainPage()
        {
            InitializeComponent();

            MapControl.NativeMap.Layers.Add(GetKaagTileLayer());
            MapControl.NativeMap.Layers.Add(new PathLayer(DebugRoute.Waypoints, Consts.MainPathLayerName));

            MapControl.NativeMap.NavigateTo(new Position(52.22002f, 4.55835f).ToMapsui());
            MapControl.NativeMap.NavigateTo(0.5972);
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