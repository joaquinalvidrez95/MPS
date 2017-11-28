using MPS.ViewModel;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Collections.ObjectModel;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;

namespace MPS
{
    internal class MessagePageViewModel : BaseViewModel
    {
        private string _message;
        private ObservableCollection<Message> _messages;

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

        public ICommand MessagePopupCommand { get; }

        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set { _messages = value; OnPropertyChanged(); }
        }


        public MessagePageViewModel()
        {
            Messages = new ObservableCollection<Message>();
            SendMessageCommand = new Command(SendMessage);
            MessagePopupCommand = new Command(OpenPopupMessage);
            MessagingCenter.Subscribe<MessagePopupViewModel, Message>(this, MessengerKeys.Message2, OnMessageAdded);
            Message = "";
        }

        private void OnMessageAdded(MessagePopupViewModel arg1, Message arg2)
        {
            Messages.Add(arg2);
        }

        private async void OpenPopupMessage()
        {
            await PopupNavigation.PushAsync(new MessagePopup());
        }

        private void SendMessage()
        {
            MessagingCenter.Send(this, MessengerKeys.Message, Message);
        }
    }
}