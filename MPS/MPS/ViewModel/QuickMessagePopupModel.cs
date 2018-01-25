using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Helper;
using MPS.Model;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class QuickMessagePopupModel : BaseViewModel
    {
        private string _text;

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
                OnPropertyChanged();
            }
        }


        public QuickMessagePopupModel()
        {
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
        }

        private async void CancelMessage()
        {
            await PopupNavigation.PopAsync();
        }

        private async void FinishMessage()
        {
            if (string.IsNullOrEmpty(Text)) return;
            await PopupNavigation.PopAsync();
            Message m = new Message { Text = Text };
            MessagingCenter.Send(this, MessengerKeys.Message, m);
        }
        
    }
}
