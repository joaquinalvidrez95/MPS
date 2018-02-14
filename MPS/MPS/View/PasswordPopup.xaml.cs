using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Helper;
using MPS.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MPS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordPopup : PopupPage
    {        
        public PasswordPopup()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<PasswordPopupModel>(this, MessengerKeys.OnPopAsync, async model => await PopupNavigation.RemovePageAsync(this));
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<PasswordPopupModel>(this, MessengerKeys.OnPopAsync);
        }
    }
}