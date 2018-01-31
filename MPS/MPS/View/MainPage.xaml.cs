using MPS.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Helper;
using Xamarin.Forms;

namespace MPS
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            //BindingContext = new MainPageModel(Navigation);
        }

      

        protected override bool OnBackButtonPressed()
        {

            //Debug.WriteLine("OnBackButtonPressed------\n");
            MessagingCenter.Send(this, MessengerKeys.OnBackButtonPressed);
            return base.OnBackButtonPressed();
            
        }
    }
}
