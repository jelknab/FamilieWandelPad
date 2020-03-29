using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FamilieWandelPad.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullScreenImageModal : ContentPage
    {
        public FullScreenImageModal(byte[] imageBytes)
        {
            InitializeComponent();
            Image.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
    }
}