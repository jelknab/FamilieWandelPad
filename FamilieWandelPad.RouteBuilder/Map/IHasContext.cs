using System.Windows.Controls;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.RouteBuilder.Editing;

namespace FamilieWandelPad.RouteBuilder.Map
{
    public interface IHasContext
    {
        void OnContextOpen(ContextMenu contextMenu, IRouteController route);
    }
}