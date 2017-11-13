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
        public MainParameters()
        {
            InitializeComponent();
        }

        private void buttonMainShowDate_Clicked(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now.ToLocalTime();
            if (DateTime.Now.IsDaylightSavingTime() == true)
            {
                now = now.AddHours(1);
            }
            string currentTime = (string.Format("Current Time: {0}", now));            
            

        }

        private void buttonMainView_Clicked(object sender, EventArgs e)
        {

        }      

        private void stepperMainBrightness_ValueChanged(object sender, ValueChangedEventArgs e)
        {

        }
    }
}