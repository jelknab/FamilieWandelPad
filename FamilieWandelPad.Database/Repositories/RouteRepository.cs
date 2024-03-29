using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using Microsoft.EntityFrameworkCore;

namespace FamilieWandelPad.Database.Repositories
{
    public class RouteRepository
    {
        public static Route GetRoute(string path)
        {
            using (var context = RouteDbContext.GetContext(path))
            {
                var route = context.Route
                                .Include(r => r.Sections).ThenInclude(s => s.Polygon)
                                .Include(r => r.Waypoints)
                                .ThenInclude(poi => (poi as PointOfInterest).Translations)
                                .FirstOrDefault()
                            ??
                            new Route()
                            {
                                Waypoints = new List<RoutePoint>(),
                                Sections = new List<Section>()
                            };
                route.RecoverFromLoading();

                return route;
            }
        }
        
        public static async Task<Route> GetRouteAsync(string path)
        {
            using (var context = RouteDbContext.GetContext(path))
            {
                var route = await context.Route
                                .Include(r => r.Sections).ThenInclude(s => s.Polygon)
                                .Include(r => r.Waypoints)
                                .ThenInclude(poi => (poi as PointOfInterest).Translations)
                                .FirstOrDefaultAsync()
                            ??
                            new Route()
                            {
                                Waypoints = new List<RoutePoint>(),
                                Sections = new List<Section>()
                            };
                route.RecoverFromLoading();

                return route;
            }
        } 

        public static void SaveRoute(string path, Route route)
        {
            route.PrepareForSaving();
            using (var context = RouteDbContext.GetContext(path))
            {
                context.Route.Add(route);
                context.SaveChanges();
            }
        }
    }
}