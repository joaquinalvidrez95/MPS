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
        //public static readonly BindableProperty ColorNameProperty = BindableProperty.Create(
        //                                                 propertyName: "ColorName",
        //                                                 returnType: typeof(string),
        //                                                 declaringType: typeof(ColorView),
        //                                                 defaultValue: "",
        //                                                 defaultBindingMode: BindingMode.TwoWay,
        //                                                 propertyChanged: ColorNamePropertyChanged);

        //private static void ColorNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (ColorView)bindable;
        //    control.button.BackgroundColor =(Color) new ColorTypeConverter().ConvertFromInvariantString((string)newValue);
        //}

        public Command ColorCommand
        {
           
            set
            {
                button.Command = value;
            }
        }
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

                button.BackgroundColor = color;           
               
            }
            get
            {
                return colorName;
            }
        }
    
   
    }
    
}