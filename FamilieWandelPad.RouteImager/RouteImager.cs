using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading;
using BruTile;
using BruTile.MbTiles;
using BruTile.Predefined;
using BruTile.Web;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.Database.Repositories;
using FamilieWandelPad.Map.MapLayers;
using FamilieWandelPad.navigation;
using Mapsui;
using Mapsui.Fetcher;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Rendering.Skia;
using Mapsui.Rendering.Skia.SkiaStyles;
using Mapsui.Styles;
using Mapsui.UI.Forms;
using Mapsui.Utilities;
using Mapsui.Widgets;
using SkiaSharp;
using SQLite;
using Polygon = Xamarin.Forms.Shapes.Polygon;

namespace FamilieWandelPad.RouteImager
{
    public class RouteImager
    {
        private static readonly string dbFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Familie Wandel Pad",
            "route.sqlite"
        );

        public RouteImager()
        {
            var route = LoadRouteFromDb();

            var area = new BoundingBox(
                SphericalMercator.FromLonLat(
                    route.Waypoints.Min(w => w.Longitude) - 0.001,
                    route.Waypoints.Min(w => w.Latitude) - 0.001
                ),
                SphericalMercator.FromLonLat(
                    route.Waypoints.Max(w => w.Longitude) + 0.001,
                    route.Waypoints.Max(w => w.Latitude) + 0.001
                )
            );


            ShootSnapshot(area, route, 2480, 3508, 1, "overview");
            ShootSnapshot(area, route, 2480 * 2, 3508 * 2, 1, "overview_big");
        }

        private void ShootSnapshot(BoundingBox boundingBox, Route route, int width,
            int height, int scale, string fileName)
        {
            var imageFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Familie Wandel Pad",
                "MapImages",
                $"{fileName}.png"
            );

            using var fileStream = new FileStream(imageFilePath, FileMode.Create, FileAccess.Write);
            using var skSurface = SKSurface.Create(
                new SKImageInfo(width, height, SKImageInfo.PlatformColorType, SKAlphaType.Unpremul)
            );
            skSurface.Canvas.Clear(Color.Transparent.ToSkia());
            skSurface.Canvas.Scale(scale, scale);
            
            var viewport = new Viewport
            {
                Center = boundingBox.Centroid,
                Width = width,
                Height = height,
                Resolution = ZoomHelper.DetermineResolution(boundingBox.Width, boundingBox.Height, width, height)
            };
            
            DrawMap(skSurface, boundingBox, viewport);
            DrawRoute(skSurface, viewport, boundingBox, route);
            DrawArrows(skSurface, viewport, route, scale);
            DrawPOIs(skSurface, viewport, route, scale);

            using var image = skSurface.Snapshot();
            using var data = image.Encode();
            data.SaveTo(fileStream);
        }

        private void DrawArrows(SKSurface skSurface, IReadOnlyViewport viewport, Route route, int scale)
        {
            const string resourceId = "FamilieWandelPad.RouteImager.Arrow_Up.png";
            var assembly = GetType().GetTypeInfo().Assembly;

            SKBitmap resourceBitmap;
            using (var stream = assembly.GetManifestResourceStream(resourceId))
            {
                resourceBitmap = SKBitmap.Decode(stream);
                resourceBitmap = resourceBitmap.Resize(new SKSizeI(25, 25), SKFilterQuality.High);
            }

            var arrowSteps = Distance.FromMeters(50);

            for (var index = 0; index < route.Waypoints.Count; index++)
            {
                var waypoint = route.Waypoints[index];
                var nextWaypoint = route.Waypoints[(index + 1) % route.Waypoints.Count];

                var a = new Vector2((float) waypoint.Latitude, (float) waypoint.Longitude);
                var b = new Vector2((float) nextWaypoint.Latitude, (float) nextWaypoint.Longitude);
                var aToB = b - a; //Vector from A to B  

                var steps = Math.Max(
                    Distance.FromMiles(waypoint.Distance(nextWaypoint)).Meters / arrowSteps.Meters, 2
                );

                for (var step = 1; step < steps; step++)
                {
                    var arrow = a + aToB * (float) (step / steps);

                    var screenPoint = viewport.WorldToScreen(
                        SphericalMercator.FromLonLat(arrow.Y, arrow.X)
                    );

                    skSurface.Canvas.Save();
                    skSurface.Canvas.RotateDegrees((float) waypoint.DegreeBearing(nextWaypoint), (float) screenPoint.X,
                        (float) screenPoint.Y);

                    skSurface.Canvas.DrawBitmap(
                        resourceBitmap,
                        (float) screenPoint.X - resourceBitmap.Width / 2f,
                        (float) screenPoint.Y - resourceBitmap.Height / 2f
                    );

                    skSurface.Canvas.Restore();
                    // skSurface.Canvas.DrawCircle((float) screenPoint.X, (float) screenPoint.Y, 10, new SKPaint() {Color = SKColors.Blue});
                }
            }
        }

        private static void DrawMap(SKSurface skSurface, BoundingBox area,
            IReadOnlyViewport viewport)
        {
            var tileFeatures = new TileMemoryLayer(KnownTileSources.Create(KnownTileSource.OpenStreetMap))
                .GetFeaturesInView(area, viewport.Resolution);

            foreach (var tileFeature in tileFeatures)
            {
                var raster = (IRaster) tileFeature.Geometry;
                var bitmapInfo = BitmapHelper.LoadBitmap(raster.Data);
                var paint = new SKPaint
                {
                    FilterQuality = SKFilterQuality.High,
                    Color = new SKColor(255, 255, 255, 255)
                };

                var destination = WorldToScreen(viewport, tileFeature.Geometry.BoundingBox);
                skSurface.Canvas.DrawImage(bitmapInfo.Bitmap, RoundToPixel(destination).ToSkia(), paint);
            }
        }

        private static void DrawRoute(SKSurface skSurface, IReadOnlyViewport viewport, BoundingBox area, Route route)
        {
            var routeLayer = GetPathLayer(route);
            var routeFeatures = routeLayer.GetFeaturesInView(area, viewport.Resolution);

            foreach (var routeFeature in routeFeatures)
            {
                foreach (var style in (StyleCollection) routeLayer.Style)
                {
                    LineStringRenderer.Draw(
                        skSurface.Canvas,
                        viewport,
                        style,
                        routeFeature,
                        routeFeature.Geometry,
                        1
                    );
                }
            }
        }

        private static void DrawPOIs(SKSurface skSurface, IReadOnlyViewport viewport, Route route, float scale)
        {
            var pois = route.Waypoints
                .OfType<PointOfInterest>()
                .OrderBy(p => p.OrderIndex)
                .ToList();

            var poiStroke = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High,
                Color = Color.Black.ToSkia(),
                StrokeWidth = 10f * scale
            };
            var poiFill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.White.ToSkia()
            };
            var textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 32f * scale
            };

            foreach (var pointOfInterest in pois)
            {
                var screenPoint = viewport.WorldToScreen(
                    SphericalMercator.FromLonLat(pointOfInterest.Longitude, pointOfInterest.Latitude)
                );
                var x = (float) screenPoint.X;
                var y = (float) screenPoint.Y;

                var text = $"{pois.IndexOf(pointOfInterest)}";
                var textBounds = new SKRect();

                textPaint.MeasureText(text, ref textBounds);

                skSurface.Canvas.DrawCircle(x, y, 30f * scale, poiFill);
                skSurface.Canvas.DrawCircle(x, y, 30f * scale, poiStroke);
                skSurface.Canvas.DrawText(text, x - textBounds.MidX, y - textBounds.MidY, textPaint);
            }
        }

        private static BoundingBox WorldToScreen(IReadOnlyViewport viewport, BoundingBox boundingBox)
        {
            var first = viewport.WorldToScreen(boundingBox.Min);
            var second = viewport.WorldToScreen(boundingBox.Max);
            return new BoundingBox
            (
                Math.Min(first.X, second.X),
                Math.Min(first.Y, second.Y),
                Math.Max(first.X, second.X),
                Math.Max(first.Y, second.Y)
            );
        }

        private static BoundingBox RoundToPixel(BoundingBox boundingBox)
        {
            return new BoundingBox(
                (float) Math.Round(boundingBox.Left),
                (float) Math.Round(Math.Min(boundingBox.Top, boundingBox.Bottom)),
                (float) Math.Round(boundingBox.Right),
                (float) Math.Round(Math.Max(boundingBox.Top, boundingBox.Bottom)));
        }

        private static MemoryLayer GetPathLayer(Route route)
        {
            var layer = new PathLayer(route.Waypoints, "route")
            {
                Style = new StyleCollection()
                {
                    new VectorStyle // Gray outer
                    {
                        Enabled = true,
                        Line = new Pen(Color.FromArgb(255, 120, 120, 120)) {PenStyle = PenStyle.Solid, Width = 12d}
                    },
                    new VectorStyle // Blue inner
                    {
                        Enabled = true,
                        Line = new Pen(Color.FromArgb(255, 100, 149, 237)) {PenStyle = PenStyle.Solid, Width = 6d}
                    }
                }
            };

            layer.Style.Enabled = true;
            return layer;
        }

        private MemoryLayer GetPoiLayer(Route route)
        {
            var layer = new MemoryLayer
            {
                DataSource = new MemoryProvider(
                    route.Waypoints
                        .OfType<PointOfInterest>()
                        .ToList()
                        .Select(poi => new Feature
                        {
                            Geometry = SphericalMercator.FromLonLat(poi.Longitude, poi.Latitude)
                        })
                ),
                IsMapInfoLayer = true,
                Style = new SymbolStyle
                {
                    BitmapId = GetBitmapIdForEmbeddedResource("FamilieWandelPad.RouteImager.Assets.Pin.svg"),
                    SymbolScale = 1d,
                    SymbolOffset = new Offset(0.0, 0.5, true),
                    Enabled = true
                }
            };

            return layer;
        }

        private int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            var image = GetType().Assembly.GetManifestResourceStream(imagePath);
            return BitmapRegistry.Instance.Register(image);
        }

        private static Route LoadRouteFromDb()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dbFilePath));

            return RouteRepository.GetRoute(dbFilePath);
        }
    }
}