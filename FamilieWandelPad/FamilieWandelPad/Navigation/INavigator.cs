using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using FamilieWandelPad.Navigation.Route.waypoints;

namespace FamilieWandelPad.navigation
{
    public interface INavigator
    {
        WayPoint FindStartingWaypoint();

        WayPoint GetNextWaypoint(WayPoint currentWayPoint);

        void NavigateTo(WayPoint target);

        void OnNavigationFinished();

        void OnLocationUpdate(object sender, PositionEventArgs e);
    }
}
