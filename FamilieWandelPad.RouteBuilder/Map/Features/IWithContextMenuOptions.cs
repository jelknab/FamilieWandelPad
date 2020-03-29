using System.Windows.Controls;
using FamilieWandelPad.RouteBuilder.Editing;

namespace FamilieWandelPad.RouteBuilder.Map.Features
{
    public interface IHasContext
    {
        void OnContextOpen(ContextMenu contextMenu, IRouteController route);
    }
}