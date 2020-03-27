using System;
using System.Linq;
using FamilieWandelPad.Database;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Tests.Navigation.Route;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FamilieWandelPad.Tests.Storing
{
    public class RouteSaveAndLoad : DatabaseTestBase
    {
        [Fact]
        public void StoreAndGetRouteTest()
        {
            var route = new RouteStub();

            using (var context = getContext())
            {
                route.PrepareForSaving();
                context.Route.Add(route);
                context.SaveChanges();
            }

            using (var context = getContext())
            {
                var loadedRoute = context.Route
                    .Include(r => r.Sections).ThenInclude(s => s.Polygon)
                    .Include(r => r.Waypoints)
                    .ThenInclude(poi => (poi as PointOfInterest).Translations)
                    .First();

                loadedRoute.RecoverFromLoading();

                Assert.Equal(route.Waypoints[3].Latitude, loadedRoute.Waypoints[3].Latitude);


                Assert.Equal(
                    (route.Waypoints[0] as PointOfInterest).Translations.First().Text,
                    (loadedRoute.Waypoints[0] as PointOfInterest).Translations.First().Text
                );

                Assert.Equal(
                    route.Waypoints[3].OrderIndex,
                    loadedRoute.Waypoints[3].OrderIndex
                );
            }
        }
    }
}