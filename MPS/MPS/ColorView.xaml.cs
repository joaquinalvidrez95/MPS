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
		public ColorView ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
          
        }
    }
}