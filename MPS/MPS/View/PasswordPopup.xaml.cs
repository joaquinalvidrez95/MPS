using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.ViewModel;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MPS.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordPopup : PopupPage
    {
        public event EventHandler Closed;
        public PasswordPopup()
        {
            InitializeComponent();
            BindingContext = new PasswordPopupModel(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}