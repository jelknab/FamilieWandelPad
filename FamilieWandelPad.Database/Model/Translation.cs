using FamilieWandelPad.Database.Model.waypoints;

namespace FamilieWandelPad.Database.Model
{
    public class Translation
    {
        public int Id { get; set; }
        
        public PointOfInterest PointOfInterest { get; set; }
        
        public string Language { get; set; }
        
        public string Text { get; set; }
    }
}