using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using Mapsui.Geometries;

namespace FamilieWandelPad.RouteBuilder.Editing
{
    public interface IRouteController
    {
        Route Route { get; set; }
        
        void AddWaypoint(Point position);
        
        void MoveWaypoint(Point position);
        
        void SelectRoutePoint(RoutePoint routePoint);
        
        void DeleteRoutePoint(RoutePoint routePoint);
        
        void ReplaceRoutePoint(RoutePoint waypoint, RoutePoint newRoutePoint);

        void Save();
    }
}