﻿using System.Linq;
using System.Threading.Tasks;
using BruTile.MbTiles;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Database.Repositories;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.navigation;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Styles;
using Mapsui.UI.Forms;
using Plugin.Geolocator;
using Xamarin.Forms;
using Color = Mapsui.Styles.Color;

namespace FamilieWandelPad.Pages
{
    public partial class MapPage
    {
        private NavigationStats NavigationStats { get; set; }

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

            MapControl.TouchStarted += async (sender, args) =>
            {
                var mapInfo = MapControl.GetMapInfo(args.ScreenPoints.First());
                if (mapInfo.Feature is PointOfInterestFeature poif)
                {
                    await poif.OnClick();
                }
            };

            var routeLayer = new PathLayer(
                route.GetEnumerable(route.Waypoints.First(), Direction.Forward),
                Consts.MainPathLayerName
            );
            routeLayer.Style = null;
            routeLayer.feature.Styles.Add(
                new VectorStyle()
                {
                    Line = new Pen(Color.FromArgb(255, 48, 78, 130))
                    {
                        PenStyle = PenStyle.Solid,
                        Width = 15d
                    }
                }
            );
            routeLayer.feature.Styles.Add(
                new VectorStyle()
                {
                    Line = new Pen(Color.FromArgb(255, 45, 115, 200))
                    {
                        PenStyle = PenStyle.Solid,
                        Width = 12d
                    }
                }
            );

            MapControl.Map.Layers.Add(routeLayer);
            NavigationStats = new NavigationStats(route);
            Navigator = new Navigator(MapControl, route, CrossGeolocator.Current, NavigationStats);

            DistanceLabel.SetBinding(Label.TextProperty, new Binding("Progress", source: NavigationStats));

            await Application.Current.MainPage.Navigation.PushModalAsync(new LookingForGpsPage());
            await Navigator.StartNavigation();
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private static TileLayer GetKaagTileLayer()
        {
            var mbTilesTileSource = new MbTilesTileSource(App.MbTileConnectionString);
            var mbTilesLayer = new TileLayer(mbTilesTileSource);
            return mbTilesLayer;
        }

        private void MenuButtonClicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new MenuPage());
        }
    }
}