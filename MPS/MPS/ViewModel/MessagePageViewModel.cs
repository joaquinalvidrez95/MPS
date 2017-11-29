using MPS.ViewModel;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;

namespace MPS
{
    public class MessagePageViewModel : BaseViewModel
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

        public ICommand DeleteCommand { get; }

        public Message SelectedItem { get; set; }

        public ICommand SendCommand { get; }
        public MessagePageViewModel()
        {
            Messages = new ObservableCollection<Message>();
            DeleteCommand = new Command<string>(DeleteMessage);
            SendCommand = new Command<string>(Send);
            SendMessageCommand = new Command(SendMessage);
            MessagePopupCommand = new Command(OpenPopupMessage);
            MessagingCenter.Subscribe<MessagePopupViewModel, Message>(this, MessengerKeys.Message2, OnMessageAdded);
            Message = "";
        }

        private void Send(string obj)
        {
            foreach (var message in Messages.ToList())
            {
                if (message.Title.Equals(obj))
                {
                    MessagingCenter.Send(this, MessengerKeys.Message, message.Text);
                }
            }
        }

        private void DeleteMessage(string obj)
        {
            foreach (var message in Messages.ToList())
            {
                if (message.Title.Equals(obj))
                {
                    Messages.Remove(message);
                }
            }
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