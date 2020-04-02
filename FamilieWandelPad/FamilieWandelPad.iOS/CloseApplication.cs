using System.Threading;
using FamilieWandelPad.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(CloseApplication))]
namespace FamilieWandelPad.iOS
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            Thread.CurrentThread.Abort();
        }
    }
}