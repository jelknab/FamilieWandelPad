using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BruTile.MbTiles;
using BruTile.Predefined;
using FamilieWandelPad.RouteDesigner.Map;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.UI.Wpf;
using SQLite;

namespace FamilieWandelPad.RouteDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public List<Point> Path { get; set; }
        public PathPointsLayer PointsLayer { get; set; }
        public PathLayer PathLayer { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            
            Path = new List<Point>();
            PointsLayer = new PathPointsLayer();
            PathLayer = new PathLayer();
            
            MapControl.Map.Layers.Add(GetKaagTileLayer());
            MapControl.Map.Layers.Add(PathLayer);
            MapControl.Map.Layers.Add(PointsLayer);
            MapControl.Info += (sender, args) =>
            {
                if (args.MapInfo.Feature is PathPointFeature)
                {
                    Console.WriteLine("feature");
                }
            };

            MapControl.MouseRightButtonDown += (sender, args) =>
            {
                var mapInfo = args.GetMapInfo(MapControl);
                
                var sphericalCoordinate = mapInfo.WorldPosition;
                Path.Add(sphericalCoordinate);
                PointsLayer.Update(Path);
                PathLayer.UpdatePath(Path);
            };
        }
        
        public TileLayer GetKaagTileLayer()
        {
            WriteResourceToFile("FamilieWandelPad.RouteDesigner.Assets.Kaag.mbtiles", "Kaag.mbtiles");
            
            var mbTilesTileSource = new MbTilesTileSource(new SQLiteConnectionString("Kaag.mbtiles"));
            var mbTilesLayer = new TileLayer(mbTilesTileSource);
            return mbTilesLayer;
        }
        
        public void WriteResourceToFile(string resourceName, string fileName)
        {
            using(var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using(var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                } 
            }
        }
    }
}