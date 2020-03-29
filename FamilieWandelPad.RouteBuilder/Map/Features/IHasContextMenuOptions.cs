using System.Windows.Controls;
using FamilieWandelPad.RouteBuilder.Controllers;
using FamilieWandelPad.RouteBuilder.Editing;

namespace FamilieWandelPad.RouteBuilder.Map.Features
{
    public interface IHasContextMenuOptions
    {
        void OnContextOpen(ContextMenu contextMenu, IRouteController route);
    }
}