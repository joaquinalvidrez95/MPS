using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MainParametersViewModel : ViewModelBase
    {
        private const double STEP_VALUE = 1.0;
        private string currentDateTime;
        private double speed;

        public ICommand DateTimeCommand{get;private set;}
        public string CurrentDateTime
        {
            get
            {
                return currentDateTime;
            }
            set
            {
                currentDateTime = value;
                OnPropertyChanged();
            }
        }

        public double Speed
        {
            get
            {
                return speed;
            }
            set
            {
                //var newStep = Math.Round(value / STEP_VALUE);               
                //speed = newStep * STEP_VALUE;
                value = Math.Round(value / STEP_VALUE);
                value = value * STEP_VALUE;
                speed = value;
                OnPropertyChanged();
            }
        }

        public MainParametersViewModel(INavigation navigation)
        {
            DateTimeCommand = new Command(showTime);            
        }

        private void showTime(object obj)
        {
            DateTime now = DateTime.Now.ToLocalTime();
            if (DateTime.Now.IsDaylightSavingTime() == true)
            {
                now = now.AddHours(1);
            }
            string currentTime = (string.Format("Current Time: {0}", now));
            CurrentDateTime= currentTime;
        }


       
    }
}
