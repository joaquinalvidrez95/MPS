using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class PasswordPopupModel : BaseViewModel
    {
        private string _password;

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged();}
        }

        public ICommand DoneCommand { get; }

        public ICommand CancelCommand { get; }

        public PasswordPopupModel()
        {
            Password = "";
            DoneCommand=new Command(StartConnection);
            CancelCommand=new Command(CancelConnection);
        }

        private async void CancelConnection()
        {
            await PopupNavigation.PopAsync();
        }

        private async void StartConnection()
        {
            await PopupNavigation.PopAsync();
        }

        protected override void Subscribe()
        {
            
        }
    }
}
