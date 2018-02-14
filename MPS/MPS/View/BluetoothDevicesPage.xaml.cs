using MPS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MPS.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BluetoothDevicesPage : ContentPage
	{
        
		public BluetoothDevicesPage ()
		{
			InitializeComponent ();
		    MessagingCenter.Subscribe<BluetoothDevicesPageModel>(this, MessengerKeys.OnPopAsync, async model => await Navigation.PopAsync());

        }

        protected override void OnDisappearing()
	    {
	        base.OnDisappearing();
	        MessagingCenter.Unsubscribe<BluetoothDevicesPageModel>(this, MessengerKeys.OnPopAsync);
	    }
    }
}