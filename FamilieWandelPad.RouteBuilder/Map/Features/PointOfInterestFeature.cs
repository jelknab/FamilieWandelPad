using System.Windows.Controls;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.RouteBuilder.Editing;

namespace FamilieWandelPad.RouteBuilder.Map.Features
{
    public class PointOfInterestFeature : WaypointFeature
    {
        public override void OnContextOpen(ContextMenu contextMenu, IRouteController route)
        {
            contextMenu.Items.Add(SelectMenuItem(route));
            contextMenu.Items.Add(DeleteMenuItem(route));
            contextMenu.Items.Add(ToWaypointMenuItem(route));
        }
        
        private MenuItem ToWaypointMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Make waypoint"};
            mi.Click += (o, eventArgs) =>
            {
                route.ReplaceRoutePoint(RoutePoint, new WayPoint(RoutePoint));
            };

            return mi;
        }
    }
}