using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using Mapsui.Geometries;

namespace FamilieWandelPad.RouteBuilder.Controllers
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
        
        void SelectSectionPoint(Section section, GeoPosition point);

        void AddSection(Point position);
        void AddSectionPoint(Point position);
        
        void RemoveSection(Section section, GeoPosition point);
        void RemoveSectionPoint(Section section, GeoPosition point);
    }
}