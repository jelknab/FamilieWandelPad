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
    public class CurrentPositionLayer : AnimatedPointLayer
    {
        public CurrentPositionLayer(Position position) : base(new LocationDotProvider(position))
        {
            Name = Consts.CurrentPositionLayerName;
        }
        
        public void Update(Position position, double rotation)
        {
            Position = position;
            Rotation = rotation;
            
            UpdateData();
        }
    }

    internal class LocationDotProvider : MemoryProvider
    {
        

        public Position Position { get; set; }
        public double Rotation { get; set; } = 0;
        
        private int BitMapId { get; }
        
        public LocationDotProvider(Position position)
        {
            const string bitmapPath = @"FamilieWandelPad.Assets.navigationArrow.svg";
            var bitmapStream = typeof(CurrentPositionLayer).GetTypeInfo().Assembly.GetManifestResourceStream(bitmapPath);
            BitMapId = BitmapRegistry.Instance.Register(bitmapStream);
            
            Position = position;
        }
        
        public void Update(Position position, double rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public override IEnumerable<IFeature> GetFeaturesInView(BoundingBox box, double resolution)
        {
            var features = new Features();
            
            features.Add(new Feature
            {
                Geometry = SphericalMercator.FromLonLat(Position.Longitude, Position.Latitude),
                Styles = new List<IStyle>()
                {
                    new SymbolStyle
                    {
                        // BitmapId = BitMapId,
                        // SymbolRotation = Rotation,
                        
                        Fill = new Brush(new Color(40, 40, 40)),
                        SymbolScale = 0.5
                    }
                }
            });

            return features;
        }
    }
}