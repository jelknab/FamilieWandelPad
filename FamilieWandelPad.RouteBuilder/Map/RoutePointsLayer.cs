using System;
using System.Collections.Generic;
using System.Linq;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using FamilieWandelPad.RouteBuilder.Map.Features;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;

namespace FamilieWandelPad.RouteBuilder.Map
{
    public class RoutePointsLayer : MemoryLayer
    {
        public RoutePointsLayer()
        {
            _memoryProvider = new MemoryProvider();
            DataSource = _memoryProvider;
            IsMapInfoLayer = true;
            Style = new SymbolStyle
            {
                Enabled = true,
                SymbolType = SymbolType.Ellipse,
                SymbolScale = 0.25,
                Fill = new Brush(new Color(40, 40, 40))
            };
        }

        private MemoryProvider _memoryProvider { get; }

        public void Update(Route route)
        {
            _memoryProvider.Clear();
            _memoryProvider.ReplaceFeatures(
                route.Waypoints.Select(routePoint =>
                {
                    switch (routePoint)
                    {
                        case WayPoint wp:
                            return new WaypointFeature
                            {
                                RoutePoint = wp,
                                Geometry = SphericalMercator.FromLonLat(wp.Latitude, wp.Longitude)
                            };
                        
                        case PointOfInterest poi:
                            return new PointOfInterestFeature
                            {
                                RoutePoint = poi,
                                Geometry = SphericalMercator.FromLonLat(poi.Latitude, poi.Longitude)
                            };
                        
                        default:
                            throw new NotImplementedException();
                    }
                })
            );

            DataHasChanged();
        }
    }
}