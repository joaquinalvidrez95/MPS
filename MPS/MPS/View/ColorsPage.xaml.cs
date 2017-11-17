using MPS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MPS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorsPage : ContentPage
    {
        public ColorsPage()
        {
            InitializeComponent();
            int initialBorderWith = 1;
            Color initialBorderColor = Color.Black;
            this.BindingContext = new ColorsPageViewModel            
            {
                ColorBackground = Color.Aqua,
                ColorLowerLine = Color.BlueViolet,
                ColorUpperLine = Color.Chartreuse,
                BorderColorLowerLineButton = initialBorderColor,
                BorderColorUpperLineButton = initialBorderColor,
                BorderColorBackgroundButton = initialBorderColor,
                BorderWidthLowerLineButton = initialBorderWith,
                BorderWidthBackgroundButton = initialBorderWith,
                BorderWidthUpperLineButton = initialBorderWith,

              
            };
        }
    }
}