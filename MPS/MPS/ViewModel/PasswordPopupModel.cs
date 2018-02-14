using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Helper;
using MPS.Model;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class PasswordPopupModel : BaseViewModel
    {
        private string _password;             
        private PasswordLoginState _loginState = PasswordLoginState.Normal;
        

        public PasswordLoginState LoginState
        {
            get => _loginState;
            set
            {
                if (_loginState == value) return;
                _loginState = value;              
                OnPropertyChanged();
            }
        }       
            
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                LoginState = PasswordLoginState.Normal;
                OnPropertyChanged();
            }
        }

        public ICommand DoneCommand { get; }

        public ICommand CancelCommand { get; }

        //public PasswordPopupModel(PopupPage page)
        //{
        //    Password = Settings.Password;
        //    DoneCommand = new Command(StartConnection);
        //    CancelCommand = new Command(CancelConnection);
        //    this._page = page;
          
        //}

        public PasswordPopupModel()
        {
            Password = Settings.Password;
            DoneCommand = new Command(StartConnection, () => false);
            CancelCommand = new Command(CancelConnection);

        }

        private void CancelConnection()
        {
            MessagingCenter.Send(this, MessengerKeys.OnLoginCancelled);
            ClosePopup();
        }

        private void ClosePopup()
        {
            MessagingCenter.Unsubscribe<MainPageModel>(this, MessengerKeys.OnPopAsync);      
            MessagingCenter.Unsubscribe<MainPageModel>(this, MessengerKeys.LoginState);
            MessagingCenter.Send(this, MessengerKeys.OnPopAsync);
        }

        private void StartConnection()
        {        
            MessagingCenter.Send(this, MessengerKeys.PasswordLogin, Password);
        }

        protected override void SubscribeMessagingCenter()
        {           
            MessagingCenter.Subscribe<MainPageModel>(this, MessengerKeys.OnPopAsync, CloseLogin);            
            MessagingCenter.Subscribe<MainPageModel, PasswordLoginState>(this, MessengerKeys.LoginState, OnLoginStateChanged);         
        }

        private void OnLoginStateChanged(MainPageModel mainPageModel, PasswordLoginState passwordLoginState)
        {
            LoginState = passwordLoginState;
            Debug.WriteLine(LoginState);
        }

        private void CloseLogin(MainPageModel mainPageModel)
        {      
            ClosePopup();
        }                 

    }
}
