using MPS.ViewModel;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using MPS.Utilities;

namespace MPS
{
    internal class MessagePageViewModel:ViewModelBase
    {
        private string message;

        public ICommand SendMessageCommand { get; private set; }
        public string Message {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();           
            }
        }

        public MessagePageViewModel(INavigation navigation)
        {
            SendMessageCommand = new Command(SendMessage);
            Message = "";
        }

        private void SendMessage()
        {           
            MessagingCenter.Send(this, MessengerKeys.Message, Message);
           
        }
    }
}