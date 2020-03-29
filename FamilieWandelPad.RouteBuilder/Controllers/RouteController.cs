using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Database.Repositories;
using FamilieWandelPad.RouteBuilder.Map;
using Point = Mapsui.Geometries.Point;

namespace FamilieWandelPad.RouteBuilder.Controllers
{
    public class RouteController : IRouteController
    {
        private static readonly string dbFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Familie Wandel Pad",
            "route.sqlite"
        );

        private readonly IMap _map;
        public Route Route { get; set; }
        public RoutePoint SelectedWaypoint { get; set; }

        public GeoPosition SelectedSectionPoint { get; set; }

        public RouteController(IMap map)
        {
            _map = map;
            Route = LoadRouteFromDb();

            SelectRoutePoint(Route.Waypoints.LastOrDefault());
        }

        private Route LoadRouteFromDb()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));

            return RouteRepository.GetRoute(dbFilePath);
        }

        public void ReplaceRoutePoint(RoutePoint waypoint, RoutePoint newRoutePoint)
        {
            Route.Waypoints[Route.Waypoints.IndexOf(waypoint)] = newRoutePoint;
            _map.UpdateRoute(Route);
        }

        public void Save()
        {
            var file = Path.GetFileNameWithoutExtension(dbFilePath);
            var newPath = dbFilePath.Replace(file, $"{file}-{DateTime.Now:yyyyMMddhhmmss}");
            File.Move(dbFilePath, newPath);

            RouteRepository.SaveRoute(dbFilePath, Route);
        }

        public void SelectSectionPoint(Section section, GeoPosition point)
        {
            SelectedSectionPoint = point;
        }

        public void AddSection(Point position)
        {
            var sectionPoint = new GeoPosition(position.Y, position.X);
            var section = new Section
            {
                Polygon = new List<GeoPosition>
                {
                    sectionPoint
                }
            };

            SelectSectionPoint(section, sectionPoint);
            Route.Sections.Add(section);

            _map.UpdateRoute(Route);
        }

        public void AddSectionPoint(Point position)
        {
            if (SelectedSectionPoint == null)
            {
                MessageBox.Show("Select a section point first");
                return;
            }

            var section = Route.Sections
                .Find(s => s.Polygon.Contains(SelectedSectionPoint));

            var sectionPoint = new GeoPosition(position.Y, position.X);

            section.Polygon.Insert(
                section.Polygon.IndexOf(SelectedSectionPoint),
                sectionPoint
            );
            
            SelectSectionPoint(section, sectionPoint);
            _map.UpdateRoute(Route);
        }

        public void RemoveSection(Section section, GeoPosition point)
        {
            Route.Sections.Remove(section);
            if (section.Polygon.Contains(SelectedSectionPoint)) SelectSectionPoint(null, null);
            
            _map.UpdateRoute(Route);
        }

        public void RemoveSectionPoint(Section section, GeoPosition point)
        {
            section.Polygon.Remove(point);
            if (point == SelectedSectionPoint) SelectSectionPoint(null, null);
            
            _map.UpdateRoute(Route);
        }

        public void MoveWaypoint(Point position)
        {
            if (SelectedWaypoint == null) return;

            SelectedWaypoint.Latitude = position.Y;
            SelectedWaypoint.Longitude = position.X;
            _map.UpdateRoute(Route);
        }

        public void SelectRoutePoint(RoutePoint routePoint)
        {
            SelectedWaypoint = routePoint;
        }

        public void DeleteRoutePoint(RoutePoint routePoint)
        {
            if (SelectedWaypoint == routePoint)
                SelectedWaypoint = null;

            Route.Waypoints.Remove(routePoint);
            _map.UpdateRoute(Route);
        }

        public void AddWaypoint(Point position)
        {
            var waypoint = new WayPoint(position.Y, position.X);
            if (SelectedWaypoint == null)
            {
                Route.Waypoints.Add(waypoint);
            }
            else
            {
                Route.Waypoints.Insert(
                    Route.Waypoints.IndexOf(SelectedWaypoint) + 1,
                    waypoint
                );
            }

            SelectRoutePoint(waypoint);
            _map.UpdateRoute(Route);
        }
    }
}