using System.Windows.Controls;
using System.Windows.Input;
using FamilieWandelPad.RouteBuilder.Controllers;
using Mapsui.UI.Wpf;

namespace FamilieWandelPad.RouteBuilder.Editing
{
    public abstract class DefaultEditor : IEditor
    {
        private readonly MapControl _mapControl;
        protected IRouteController RouteController;

        public DefaultEditor(MapControl mapControl, IRouteController routeController)
        {
            _mapControl = mapControl;
            RouteController = routeController;
        }

        public virtual void OnSelected()
        {
            _mapControl.MouseRightButtonDown += MapControlOnMouseRightButtonDown;
        }

        public virtual void OnDeselected()
        {
            _mapControl.MouseRightButtonDown -= MapControlOnMouseRightButtonDown;
            RouteController.SelectRoutePoint(null);
        }
        
        public abstract void MapControlOnMouseRightButtonDown(object sender, MouseButtonEventArgs e);
        
        protected MenuItem SaveRouteMenuItem()
        {
            var saveMenuItem = new MenuItem()
            {
                Header = "Save"
            };
            saveMenuItem.Click += (o, eventArgs) => { RouteController.Save(); };
            
            return saveMenuItem;
        }
    }
}