using System.Windows.Input;

namespace FamilieWandelPad.RouteBuilder.Editing
{
    public interface IEditor
    {
        void OnSelected();

        void OnDeselected();
        
        void MapControlOnMouseRightButtonDown(object sender, MouseButtonEventArgs e);
    }
}