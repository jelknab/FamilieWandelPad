using System.Collections.Generic;
using System.Diagnostics;
using Mapsui.Layers;
using SQLite;
using BruTile.MbTiles;
using FamilieWandelPad.MapLayers;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Navigation.Route.waypoints;
using Mapsui.Forms.Extensions;
using Mapsui.Geometries;
using Mapsui.UI;
using Xamarin.Forms.Maps;

namespace FamilieWandelPad
{
    public partial class MainPage
    {

        public MainPage()
        {
            InitializeComponent();

            var mapControl = new MapsUIView();
            mapControl.NativeMap.Layers.Add(GetKaagTileLayer());
            mapControl.NativeMap.Layers.Add(new PathLayer(new Route()
            {
                Waypoints = new List<WayPoint>()
                {
                    new WayPoint(52.22002f, 4.55835f),
                    new WayPoint(52.21937f, 4.55812f),
                    new WayPoint(52.21944f, 4.55774f),
                    new WayPoint(52.21975f, 4.55711f),
                    new WayPoint(52.21985f, 4.55682f),
                }
            }));
            
            mapControl.NativeMap.NavigateTo(new Position(52.22002f, 4.55835f).ToMapsui());
            
            ContentGrid.Children.Add(mapControl);

            var navigator = new Navigator(mapControl);
            // var start = navigator.FindStartingWaypoint();
            // navigator.NavigateTo(start);
        }

        public static TileLayer GetKaagTileLayer()
        {
            var mbTilesTileSource = new MbTilesTileSource(App.MbTileConnectionString);
            var mbTilesLayer = new TileLayer(mbTilesTileSource);
            return mbTilesLayer;
        }
    }
}