using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class QuickMessagePopupModel : BaseViewModel
    {
        private string _text;
        private int _leftCharacters;


        public ICommand DoneCommand
        {
            get;
        }

        public ICommand CancelCommand { get; }

        public string Text
        {
            get => _text;
            set
            {                
                _text = value;
                LeftCharacters = (int)Application.Current.Resources["MaxMessageLength"] - Text.Length;
                OnPropertyChanged();
            }
        }

        public int LeftCharacters
        {
            get => _text == null ? (int)Application.Current.Resources["MaxMessageLength"] : _leftCharacters;
            set { _leftCharacters = value; OnPropertyChanged(); }
        }

        public QuickMessagePopupModel()
        {
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
            //_text = "";
            //for (int i = 0; i < 195; i++)
            //{
            //    Text += "9";
            //}
        }

        private async void CancelMessage()
        {
            await PopupNavigation.PopAsync();
        }

        private async void FinishMessage()
        {
            if (string.IsNullOrEmpty(Text)) return;
            await PopupNavigation.PopAsync();

            MessagingCenter.Send(this, MessengerKeys.QuickMessage, Text);
        }
    }
}
