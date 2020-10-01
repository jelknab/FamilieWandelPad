using System.IO;
using System.Reflection;
using BruTile.MbTiles;
using BruTile.Predefined;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.RouteBuilder.Controllers;
using FamilieWandelPad.RouteBuilder.Editing;
using FamilieWandelPad.RouteBuilder.Map;
using Mapsui;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Styles;
using SQLite;
using IMap = FamilieWandelPad.RouteBuilder.Map.IMap;

namespace FamilieWandelPad.RouteBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMap
    {
        public IRouteController RouteController { get; set; }
        
        public RoutePointsLayer RoutePointsLayer { get; set; } = new RoutePointsLayer();
        public RouteLayer RouteLayer { get; set; } = new RouteLayer();
        
        public SectionPointsLayer SectionPointsLayer { get; set; } = new SectionPointsLayer();
        public SectionsLayer SectionsLayer { get; set; } = new SectionsLayer();
        
        private IEditor _activeEditor { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            RouteController = new RouteController(this);

            UpdateRoute(RouteController.Route);

            // MapControl.Map.Layers.Add(GetKaagTileLayer());
            MapControl.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.OpenStreetMap)));
            MapControl.Map.Layers.Add(RouteLayer);
            MapControl.Map.Layers.Add(RoutePointsLayer);
            MapControl.Map.Layers.Add(SectionsLayer);
            MapControl.Map.Layers.Add(SectionPointsLayer);

            MapControl.Map.Home = navigator => navigator.NavigateTo(SphericalMercator.FromLonLat(4.55835, 52.22002), 6);

            var wayPointEditor = new WaypointEditor(MapControl, RouteController);
            var sectionEditor = new SectionEditor(MapControl, RouteController);
            sectionEditor.OnDeselected();
            
            WaypointModeOption.Checked += (sender, args) => SetEditor(wayPointEditor);
            SectionModeOption.Checked += (sender, args) => SetEditor(sectionEditor);
            ScreenshotButton.Click += (sender, args) => Screenshot();

            WaypointModeOption.IsChecked = true;
        }

        private void Screenshot()
        {
            // var viewport = new Viewport {Center = SphericalMercator.FromLonLat(4.55835, 52.22002), Width = 200, Height = 200, Resolution = 6};
            using (var fileStream = new FileStream("test.png", FileMode.Create, FileAccess.Write))
            {
                MapControl.Renderer.RenderToBitmapStream(MapControl.Viewport, MapControl.Map.Layers, Color.White).WriteTo(fileStream);
            }
        }

        private void SetEditor(IEditor editor)
        {
            _activeEditor?.OnDeselected();
            _activeEditor = editor;
            _activeEditor.OnSelected();
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
            
            SectionPointsLayer.Update(route);
            SectionsLayer.UpdateSections(route);
        }
    }
}