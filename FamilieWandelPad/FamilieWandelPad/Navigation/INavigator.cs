using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilieWandelPad.navigation
{
    public interface INavigator
    {
        Task StartNavigation();

        void OnNavigationFinished();

        void OnLocationUpdate(object sender, PositionEventArgs e);
    }
}
