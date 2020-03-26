using System.Collections.Generic;
using System.Reflection;
using BruTile;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Map.MapLayers
{
    public class PositionLayer : MemoryLayer
    {
        private readonly SymbolStyle _style;

        public Position Position { get; set; }
        public double Rotation { get; set; } = 0;
        
        private int BitMapId { get; }
        private Feature LocationFeature { get; set; }
        
        public PositionLayer(Position originalPosition, SymbolStyle style)
        {
            _style = style;
            Position = originalPosition;

            DataSource = new MemoryProvider(GetFeatures());
            Style = null;
        }
        
        public void Update(Position position, double rotation)
        {
            Position = position;
            Rotation = rotation;
            
            LocationFeature.RenderedGeometry.Clear();
            LocationFeature.Geometry = SphericalMercator.FromLonLat(Position.Longitude, Position.Latitude);
            _style.SymbolRotation = rotation;
            
            DataHasChanged();
        }

        private Features GetFeatures()
        {
            LocationFeature = new Feature
            {
                Geometry = SphericalMercator.FromLonLat(Position.Longitude, Position.Latitude),
                Styles = new List<IStyle>
                {
                    _style
                }
            };
            
            return new Features {LocationFeature};
        }
    }
}