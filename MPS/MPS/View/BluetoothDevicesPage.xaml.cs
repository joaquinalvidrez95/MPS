﻿using MPS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            BindingContext = new BluetoothDevicesViewModel(Navigation);
            
        }
	}
}