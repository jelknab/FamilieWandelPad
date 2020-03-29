using System.Collections.Generic;
using System.Windows.Controls;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.RouteBuilder.Controllers;
using FamilieWandelPad.RouteBuilder.Editing;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteBuilder.Map.Features
{
    public class PointOfInterestFeature : WaypointFeature
    {
        public PointOfInterestFeature()
        {
            Styles = new List<IStyle>()
            {
                new SymbolStyle
                {
                    Enabled = true,
                    SymbolType = SymbolType.Ellipse,
                    SymbolScale = 0.25,
                    Fill = new Brush(new Color(40, 255, 40))
                }
            };
        }
        
        public override void OnContextOpen(ContextMenu contextMenu, IRouteController route)
        {
            contextMenu.Items.Add(EditMenuItem(route));
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

        private MenuItem EditMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Edit"};
            mi.Click += (o, eventArgs) =>
            {
                var poiWindow = new PoiWindow(route, RoutePoint as PointOfInterest);
                poiWindow.Show();
            };

            return mi;
        }
    }
}