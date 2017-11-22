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
    public partial class MainParameters : ContentPage
    {
        private const double STEP_VALUE = 1.0;

        public MainParameters()
        {
            InitializeComponent();
            BindingContext = new MainParametersViewModel();
        }
     

        private void slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / STEP_VALUE);

            slider.Value = newStep * STEP_VALUE;

        }
    }
}