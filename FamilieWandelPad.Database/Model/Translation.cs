using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using FamilieWandelPad.Database.Model.waypoints;

namespace FamilieWandelPad.Database.Model
{
    public class Translation : INotifyPropertyChanged
    {
        public int Id { get; set; }
        
        public PointOfInterest PointOfInterest { get; set; }
        
        public string Language { get; set; }

        public string Text { get; set; }
        
        [NotMapped]
        public String TextProperty
        {
            get => Text;
            set
            {
                Text = value;
                OnPropertyChanged("TextProperty");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?
                .Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}