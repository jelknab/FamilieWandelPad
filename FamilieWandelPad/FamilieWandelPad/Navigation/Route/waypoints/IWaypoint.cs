using System;
using System.Collections.Generic;
using System.Text;

namespace FamilieWandelPad.navigation
{
    interface IWaypoint
    {
        void OnArrival(INavigator navigator);
    }
}
