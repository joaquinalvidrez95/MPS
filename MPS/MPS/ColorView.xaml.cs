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
	public partial class ColorView : ContentView
	{
        string colorName;
        ColorTypeConverter colorTypeConv = new ColorTypeConverter();
        public ColorView ()
		{
			InitializeComponent ();
		}

        public string ColorName
        {
            set
            {
                // Set the name.
                colorName = value;
               
                // Get the actual Color and set the other views.
                Color color = (Color)colorTypeConv.ConvertFromInvariantString(colorName);
//                Color color = (Color)colorTypeConv.ConvertFrom(colorName);

                button.BackgroundColor = color;
               
            }
            get
            {
                return colorName;
            }
        }
    

    private void Button_Clicked(object sender, EventArgs e)
        {
          
        }
    }
}