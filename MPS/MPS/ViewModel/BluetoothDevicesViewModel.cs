using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class BluetoothDevicesViewModel : ViewModelBase
    {
        private INavigation navigation;
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        public BluetoothDevicesViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            MessagingCenter.Subscribe<MainViewModel, string>(this, "Hi", (sender, arg) => {
                // do something whenever the "Hi" message is sent
                // using the 'arg' parameter which is a string
                Text = arg;
            });
        }
    }
}
