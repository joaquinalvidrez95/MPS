using MPS.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand BluetoothConnectionCommand { get; private set; }
        public INavigation Navigation { get; }

        public MainViewModel(INavigation navigation)
        {
            BluetoothConnectionCommand = new Command(goToBluetoothDevicesPageAsync);
            Navigation = navigation;
            
        }

        private async void goToBluetoothDevicesPageAsync()
        {
           
            await Navigation.PushAsync(new BluetoothDevicesPage());
            MessagingCenter.Send(this, "Hi", "Que onda");
        }

        
    }
}
