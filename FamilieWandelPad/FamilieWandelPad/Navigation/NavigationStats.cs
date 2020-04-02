using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using FamilieWandelPad.Annotations;
using FamilieWandelPad.Database.Model;
using Mapsui.UI.Forms;

namespace FamilieWandelPad.navigation
{
    public class NavigationStats : INotifyPropertyChanged
    {
        public NavigationStats(Route route)
        {
            for (int i = 0, j = 1; j < route.Waypoints.Count; i++, j++)
                _routeLengthKm += route.Waypoints[i].Distance(route.Waypoints[j]);
            
            _routeLengthKm += route.Waypoints.Last().Distance(route.Waypoints.First());
            
            _routeLengthKm = Distance.FromMiles(_routeLengthKm).Kilometers;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _routeLengthKm;

        public double RouteLengthKm
        {
            get => _routeLengthKm;
            set
            {
                _routeLengthKm = value; 
                OnPropertyChanged(nameof(RouteLengthKm));
            }
        }

        private double _kmWalked;

        public double KmWalked
        {
            get => _kmWalked;
            set
            {
                _kmWalked = value;
                Progress = $"{value:0.##}/{_routeLengthKm:0.##} KM";
                OnPropertyChanged(nameof(KmWalked));
            }
        }

        private string _progress;

        public string Progress
        {
            get => _progress;
            set
            {
                _progress = value; 
                OnPropertyChanged(nameof(Progress));
            }
        }
    }
}