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
        private readonly PopupPage _page;      
        private PasswordLoginState _loginState;
        

        public PasswordLoginState LoginState
        {
            get => _loginState;
            set
            {
                if (_loginState == value) return;
                _loginState = value;
                CanConnect = value != PasswordLoginState.WaitingForRequest;
                OnPropertyChanged();
            }
        }       

        public bool CanConnect
        {
            get => LoginState != PasswordLoginState.WaitingForRequest && Password?.Length == (int)Application.Current.Resources["PasswordLength"];
            set => OnPropertyChanged();
        }


        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                CanConnect = Password?.Length == (int)Application.Current.Resources["PasswordLength"];                
                LoginState = PasswordLoginState.Normal;
                OnPropertyChanged();
            }
        }

        public ICommand DoneCommand { get; }

        public ICommand CancelCommand { get; }

        public PasswordPopupModel(PopupPage page)
        {
            Password = Settings.Password;
            DoneCommand = new Command(StartConnection);
            CancelCommand = new Command(CancelConnection);
            this._page = page;
          
        }
     
        private void CancelConnection()
        {
            MessagingCenter.Send(this, MessengerKeys.OnLoginCancelled);
            ClosePopup();
        }

        private async void ClosePopup()
        {
            MessagingCenter.Unsubscribe<MainPageModel>(this, MessengerKeys.ClosePasswordLogin);      
            MessagingCenter.Unsubscribe<MainPageModel>(this, MessengerKeys.LoginState);
            await PopupNavigation.RemovePageAsync(_page);
        }

        private void StartConnection()
        {        
            MessagingCenter.Send(this, MessengerKeys.PasswordLogin, Password);
        }

        protected override void SubscribeMessagingCenter()
        {           
            MessagingCenter.Subscribe<MainPageModel>(this, MessengerKeys.ClosePasswordLogin, CloseLogin);            
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
