using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using BruTile.MbTiles;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.RouteBuilder.Editing;
using FamilieWandelPad.RouteBuilder.Map;
using FamilieWandelPad.RouteBuilder.Map.Features;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.UI;
using Mapsui.UI.Wpf;
using SQLite;

namespace FamilieWandelPad.RouteBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMap
    {
        public IRouteController RouteController { get; set; }
        public RoutePointsLayer RoutePointsLayer { get; set; }
        public RouteLayer RouteLayer { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            RouteController = new RouteController(this);

            RoutePointsLayer = new RoutePointsLayer();
            RouteLayer = new RouteLayer();
            
            UpdateRoute(RouteController.Route);

            MapControl.Map.Layers.Add(GetKaagTileLayer());
            MapControl.Map.Layers.Add(RouteLayer);
            MapControl.Map.Layers.Add(RoutePointsLayer);

            MapControl.MouseRightButtonDown += (sender, args) =>
            {
                var mapInfo = args.GetMapInfo(MapControl);
                var position = SphericalMercator.ToLonLat(mapInfo.WorldPosition.X, mapInfo.WorldPosition.Y);

                var cm = new ContextMenu();

                if (mapInfo.Feature is IHasContext feature)
                {
                    feature.OnContextOpen(cm, RouteController);
                }
                else
                {
                    cm.Items.Add(NewWaypointMenuItem(position));
                    cm.Items.Add(MoveWaypointMenuItem(position));
                }

                cm.Items.Add(SaveRouteMenuItem());
                
                cm.IsOpen = true;
            };
        }

        private MenuItem NewWaypointMenuItem(Point position)
        {
            var wayPointMenuItem = new MenuItem()
            {
                Header = "New waypoint"
            };
            wayPointMenuItem.Click += (o, eventArgs) =>
            {
                RouteController.AddWaypoint(position);
            };
            return wayPointMenuItem;
        }
        
        private MenuItem MoveWaypointMenuItem(Point position)
        {
            var wayPointMenuItem = new MenuItem()
            {
                Header = "Move waypoint"
            };
            wayPointMenuItem.Click += (o, eventArgs) =>
            {
                RouteController.MoveWaypoint(position);
            };
            return wayPointMenuItem;
        }
        
        private MenuItem SaveRouteMenuItem()
        {
            var saveMenuItem = new MenuItem()
            {
                Header = "Save"
            };
            saveMenuItem.Click += (o, eventArgs) => { RouteController.Save(); };
            
            return saveMenuItem;
        }

        public TileLayer GetKaagTileLayer()
        {
            WriteResourceToFile("FamilieWandelPad.RouteBuilder.Assets.Kaag.mbtiles", "Kaag.mbtiles");

            var mbTilesTileSource = new MbTilesTileSource(new SQLiteConnectionString("Kaag.mbtiles"));
            var mbTilesLayer = new TileLayer(mbTilesTileSource);
            return mbTilesLayer;
        }

        public void WriteResourceToFile(string resourceName, string fileName)
        {
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        public void UpdateRoute(Route route)
        {
            RoutePointsLayer.Update(route);
            RouteLayer.UpdatePath(route);
        }
    }
}