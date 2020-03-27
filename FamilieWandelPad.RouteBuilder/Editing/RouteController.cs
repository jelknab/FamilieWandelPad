using System;
using System.IO;
using System.Linq;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Database.Repositories;
using FamilieWandelPad.RouteBuilder.Map;
using HarfBuzzSharp;
using Mapsui.Geometries;

namespace FamilieWandelPad.RouteBuilder.Editing
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

        public void MoveWaypoint(Point position)
        {
            if (SelectedWaypoint == null) return;
            
            SelectedWaypoint.Latitude = position.X;
            SelectedWaypoint.Longitude = position.Y;
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
            var waypoint = new WayPoint(position.X, position.Y);
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