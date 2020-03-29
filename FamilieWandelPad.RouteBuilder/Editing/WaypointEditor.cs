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
    public class WaypointEditor : DefaultEditor
    {
        private readonly MapControl _mapControl;

        public WaypointEditor(MapControl mapControl, IRouteController routeController) : base(mapControl, routeController)
        {
            _mapControl = mapControl;
        }

        public override void OnSelected()
        {
            _mapControl.Map.Layers
                .First(layer => layer is RoutePointsLayer)
                .Enabled = true;
            
            base.OnSelected();
        }

        public override void OnDeselected()
        {
            _mapControl.Map.Layers
                .First(layer => layer is RoutePointsLayer)
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
                contextMenu.Items.Add(NewWaypointMenuItem(position));
                contextMenu.Items.Add(MoveWaypointMenuItem(position));
            }

            contextMenu.Items.Add(SaveRouteMenuItem());
            
            contextMenu.IsOpen = true;
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
    }
}