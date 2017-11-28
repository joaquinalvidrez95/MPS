using MPS.ViewModel;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using MPS.Utilities;

namespace MPS
{
    internal class MessagePageViewModel : ViewModelBase
    {
        private string _message;

        public ICommand SendMessageCommand { get; }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public MessagePageViewModel()
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