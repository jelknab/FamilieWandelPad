using System.Windows.Controls;
using FamilieWandelPad.Database.Model;
using FamilieWandelPad.RouteBuilder.Controllers;
using Mapsui.Providers;

namespace FamilieWandelPad.RouteBuilder.Map.Features
{
    public class SectionPointFeature : Feature, IHasContextMenuOptions
    {
        public GeoPosition Point { get; set; }
        public Section Section { get; set; }

        public virtual void OnContextOpen(ContextMenu contextMenu, IRouteController route)
        {
            contextMenu.Items.Add(SelectSectionPointMenuItem(route));
            contextMenu.Items.Add(RemoveSectionPointMenuItem(route));
            contextMenu.Items.Add(RemoveSectionMenuItem(route));
        }

        private MenuItem SelectSectionPointMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Select"};
            mi.Click += (o, eventArgs) => { route.SelectSectionPoint(Section, Point); };

            return mi;
        }
        
        private MenuItem RemoveSectionPointMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Remove point"};
            mi.Click += (o, eventArgs) => { route.RemoveSectionPoint(Section, Point); };

            return mi;
        }
        
        private MenuItem RemoveSectionMenuItem(IRouteController route)
        {
            var mi = new MenuItem {Header = "Remove section"};
            mi.Click += (o, eventArgs) => { route.RemoveSection(Section, Point); };

            return mi;
        }
    }
}