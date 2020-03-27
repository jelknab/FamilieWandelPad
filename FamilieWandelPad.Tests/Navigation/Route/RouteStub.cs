using System.Collections.Generic;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;

namespace FamilieWandelPad.Tests.Navigation.Route
{
    public class RouteStub : Database.Model.Route
    {
        public static List<RoutePoint> TestWaypoints = new List<RoutePoint>
        {
            new PointOfInterest(52.21963, 4.56103) {Translations = new List<Translation> {new Translation{Text = "Buitenkaag start"}}},
            new WayPoint(52.22002, 4.55834),
            new WayPoint(52.21929, 4.55809),
            new PointOfInterest(52.21901, 4.56000) {Translations = new List<Translation> {new Translation{Text = "Buitenkaag end"}}},
            new PointOfInterest(52.21852, 4.55982) {Translations = new List<Translation> {new Translation{Text = "Kaag start"}}},
            new WayPoint(52.21858, 4.55931),
            new WayPoint(52.21834, 4.55916),
            new WayPoint(52.21824, 4.55964),
            new PointOfInterest(52.21851, 4.55983) {Translations = new List<Translation> {new Translation{Text = "Kaag end"}}},
            new PointOfInterest(52.21899, 4.56004) {Translations = new List<Translation> {new Translation{Text = "Buitenkaag 1 start"}}},
            new WayPoint(52.21892, 4.56077),
            new PointOfInterest(52.21962, 4.56102) {Translations = new List<Translation> {new Translation{Text = "Buitenkaag 1 end"}}}
        };

        public static List<Section> TestSections = new List<Section>
        {
            new Section
            {
                Name = "Kaag",
                Polygon = new List<GeoPosition>
                {
                    new GeoPosition(52.21906, 4.55767),
                    new GeoPosition(52.21858, 4.56107),
                    new GeoPosition(52.21747, 4.56090),
                    new GeoPosition(52.21825, 4.55800)
                }
            },
            new Section
            {
                Name = "Buitenkaag",
                Polygon = new List<GeoPosition>
                {
                    new GeoPosition(52.21906, 4.55767),
                    new GeoPosition(52.21858, 4.56107),
                    new GeoPosition(52.21975, 4.56140),
                    new GeoPosition(52.22030, 4.55795),
                    new GeoPosition(52.21948, 4.55741)
                }
            }
        };

        public RouteStub()
        {
            Waypoints = TestWaypoints;
            Sections = TestSections;
        }
    }
}