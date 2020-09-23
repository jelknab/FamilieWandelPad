using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BruTile.MbTiles;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.navigation;
using Mapsui.Layers;
using Mapsui.Styles;
using Microsoft.AppCenter.Crashes;
using Plugin.Geolocator;
using Xamarin.Essentials;
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

            DeviceDisplay.KeepScreenOn = true;

            MapView.Map.Layers.Add(GetKaagTileLayer());
        }

        public Navigator Navigator { get; set; }

        protected override async void OnAppearing()
        {
            if (Navigator != null) return;

            await Application.Current.MainPage.Navigation.PushModalAsync(new LookingForGpsPage());

            Route route = null;

            try
            {
                route = await _routeTask;
            } catch (Exception e)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Category", "Route parsing" }
                };
                Crashes.TrackError(e, properties);

                return;
            }

            try
            {
                MapView.Info += async (sender, args) =>
                {
                    args.Handled = true;
                    if (args.MapInfo.Feature is PointOfInterestFeature poif)
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

                MapView.Map.Layers.Add(routeLayer);
            } catch (Exception e)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Category", "Map styling" }
                };
                Crashes.TrackError(e, properties);

                return;
            }

            try
            {
                NavigationStats = new NavigationStats(route);
                Navigator = new Navigator(MapView, route, CrossGeolocator.Current, NavigationStats);

                DistanceLabel.SetBinding(Label.TextProperty, new Binding("Progress", source: NavigationStats));

                if (await Navigator.StartNavigation())
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            } catch (Exception e)
            {
                var properties = new Dictionary<string, string>
                {
                    { "Category", "Navigation" }
                };
                Crashes.TrackError(e, properties);

                return;
            }

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
            Application.Current.MainPage.Navigation.PushAsync(new MenuPage(Navigator));
        }
    }
}