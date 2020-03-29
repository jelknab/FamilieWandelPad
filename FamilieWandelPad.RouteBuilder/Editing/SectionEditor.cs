using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using FamilieWandelPad.RouteBuilder.Controllers;
using FamilieWandelPad.RouteBuilder.Map;
using FamilieWandelPad.RouteBuilder.Map.Features;
using Mapsui.Geometries;
using Mapsui.Projection;
using Mapsui.UI.Wpf;

namespace FamilieWandelPad.RouteBuilder.Editing
{
    public class SectionEditor : DefaultEditor
    {
        private readonly MapControl _mapControl;

        public SectionEditor(MapControl mapControl, IRouteController routeController) : base(mapControl, routeController)
        {
            _mapControl = mapControl;
        }

        public override void OnSelected()
        {
            _mapControl.Map.Layers
                .First(layer => layer is SectionPointsLayer)
                .Enabled = true;
            
            base.OnSelected();
        }

        public override void OnDeselected()
        {
            _mapControl.Map.Layers
                .First(layer => layer is SectionPointsLayer)
                .Enabled = false;
            
            base.OnDeselected();
        }

        public override void MapControlOnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mapInfo = e.GetMapInfo(_mapControl);
            var position = SphericalMercator.ToLonLat(mapInfo.WorldPosition.X, mapInfo.WorldPosition.Y);
            
            var contextMenu = new ContextMenu();
            
            if (mapInfo.Feature is IHasContextMenuOptions feature)
            {
                feature.OnContextOpen(contextMenu, RouteController);
            }
            else
            {
                contextMenu.Items.Add(NewSectionPointMenuItem(position));
                contextMenu.Items.Add(NewSectionMenuItem(position));
            }

            contextMenu.Items.Add(SaveRouteMenuItem());
            
            contextMenu.IsOpen = true;
        }
        
        private MenuItem NewSectionMenuItem(Point position)
        {
            var mi = new MenuItem {Header = "New section"};
            mi.Click += (o, eventArgs) => { RouteController.AddSection(position); };

            return mi;
        }
        
        private MenuItem NewSectionPointMenuItem(Point position)
        {
            var mi = new MenuItem {Header = "Add section point"};
            mi.Click += (o, eventArgs) => { RouteController.AddSectionPoint(position); };

            return mi;
        }
    }
}