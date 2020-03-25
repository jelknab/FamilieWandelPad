using System.Collections.Generic;
using FamilieWandelPad.navigation;
using FamilieWandelPad.Navigation.Route;
using FamilieWandelPad.Navigation.Route.waypoints;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Tests.Navigation.Route
{
    public class RouteStub : FamilieWandelPad.Navigation.Route.Route
    {
        public static List<WayPoint> TestWaypoints = new List<WayPoint>
        {
            new PointOfInterest(52.21963, 4.56103) {Description = "Buitenkaag start"},
            new WayPoint(52.22002, 4.55834),
            new WayPoint(52.21929, 4.55809),
            new PointOfInterest(52.21901, 4.56000) {Description = "Buitenkaag end"},
            new PointOfInterest(52.21852, 4.55982) {Description = "Kaag start"},
            new WayPoint(52.21858, 4.55931),
            new WayPoint(52.21834, 4.55916),
            new WayPoint(52.21824, 4.55964),
            new PointOfInterest(52.21851, 4.55983) {Description = "Kaag end"},
            new PointOfInterest(52.21899, 4.56004) {Description = "Buitenkaag 1 start"},
            new WayPoint(52.21892, 4.56077),
            new PointOfInterest(52.21962, 4.56102) {Description = "Buitenkaag 1 end"}
        };

        public static List<Section> TestSections = new List<Section>
        {
            new Section
            {
                Name = "Kaag",
                Polygon = new List<Position>
                {
                    new Position(52.21906, 4.55767),
                    new Position(52.21858, 4.56107),
                    new Position(52.21747, 4.56090),
                    new Position(52.21825, 4.55800)
                }
            },
            new Section
            {
                Name = "Buitenkaag",
                Polygon = new List<Position>
                {
                    new Position(52.21906, 4.55767),
                    new Position(52.21858, 4.56107),
                    new Position(52.21975, 4.56140),
                    new Position(52.22030, 4.55795),
                    new Position(52.21948, 4.55741)
                }
            }
        };
        
        public RouteStub() : base(TestWaypoints, TestSections)
        {
            
        }
    }
}