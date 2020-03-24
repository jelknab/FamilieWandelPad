using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;

namespace FamilieWandelPad.Tests.Navigation
{
    public class CrossGeoLocatorMock :  IGeolocator
    {
        public double DesiredAccuracy { get; set; }
        public bool IsListening { get; set; }
        public bool SupportsHeading { get; }
        public bool IsGeolocationAvailable { get; }
        public bool IsGeolocationEnabled { get; }
        public event EventHandler<PositionErrorEventArgs> PositionError;
        public event EventHandler<PositionEventArgs> PositionChanged;
        
        public Position Position { get; set; }

        public CrossGeoLocatorMock(Position start)
        {
            Position = start;
        }

        public void UpdatePosition(Position position)
        {
            Position = position;
            PositionChanged?.Invoke(this, new PositionEventArgs(position));
        }

        public async Task<Position> GetLastKnownLocationAsync()
        {
            return Position;
        }

        public async Task<Position> GetPositionAsync(TimeSpan? timeout = null, CancellationToken? token = null, bool includeHeading = false)
        {
            return Position;
        }

        public Task<IEnumerable<Address>> GetAddressesForPositionAsync(Position position, string mapKey = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Position>> GetPositionsForAddressAsync(string address, string mapKey = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> StartListeningAsync(TimeSpan minimumTime, double minimumDistance, bool includeHeading = false,
            ListenerSettings listenerSettings = null)
        {
            IsListening = true;
            return true;
        }

        public Task<bool> StopListeningAsync()
        {
            throw new NotImplementedException();
        }
    }
}