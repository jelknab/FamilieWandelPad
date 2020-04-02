using FamilieWandelPad.Droid;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApplication))]
namespace FamilieWandelPad.Droid
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            CrossCurrentActivity.Current.Activity.FinishAffinity();
        }
    }
}