using System.Windows.Controls;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.RouteBuilder.Editing;
using Mapsui.Providers;

namespace FamilieWandelPad.RouteBuilder.Map.Features
{
    public class WaypointFeature : Feature, IHasContext
    {
        public RoutePoint RoutePoint { get; set; }

        public virtual void OnContextOpen(ContextMenu contextMenu, IRouteController route)
        {
            contextMenu.Items.Add(SelectMenuItem(route));
            contextMenu.Items.Add(DeleteMenuItem(route));
            contextMenu.Items.Add(ToPoiMenuItem(route));
        }

        private MenuItem ToPoiMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Make POI"};
            mi.Click += (o, eventArgs) =>
            {
                route.ReplaceRoutePoint(RoutePoint, new PointOfInterest(RoutePoint));
            };

            return mi;
        }

        protected MenuItem SelectMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Select"};
            mi.Click += (o, eventArgs) => { route.SelectRoutePoint(RoutePoint); };

            return mi;
        }

        protected MenuItem DeleteMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Delete"};
            mi.Click += (o, eventArgs) => { route.DeleteRoutePoint(RoutePoint); };

            return mi;
        }
    }
}