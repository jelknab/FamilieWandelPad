using System.Collections.Generic;
using System.Linq;
using FamilieWandelPad.Database.Model.waypoints;

namespace FamilieWandelPad.Database.Model
{
    public class Route
    {
        public int Id { get; set; }
        
        public List<RoutePoint> Waypoints { get; set; }

        public List<Section> Sections { get; set; }

        public void PrepareForSaving()
        {
            Id = 0;
            
            Waypoints.ForEach(point =>
            {
                point.OrderIndex = Waypoints.IndexOf(point);
                point.Id = 0;

                if (point is PointOfInterest poi)
                {
                    poi.Translations?.ForEach(t => t.Id = 0);
                } 
            });
            Sections.ForEach(section => section.PrepareForSaving());
        }

        public void RecoverFromLoading()
        {
            Waypoints = Waypoints
                .OrderBy(waypoint => waypoint.OrderIndex)
                .ToList();
            Sections.ForEach(section => section.RecoverFromLoading());
        }
    }
}