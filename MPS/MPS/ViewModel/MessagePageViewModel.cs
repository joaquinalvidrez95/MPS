using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MessagePageViewModel : BaseViewModel
    {
        private string _message;
        private ObservableCollection<Message> _messages;

        public ICommand SendMessageCommand { get; }
        public ICommand ItemTappedCommand { get; }
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
            ItemTappedCommand = new Command(EditMessage);
            Messages = new ObservableCollection<Message>(new MessagesRepository().Messages.ToList());
            DeleteCommand = new Command<string>(DeleteMessage);
            SendCommand = new Command<string>(Send);
            SendMessageCommand = new Command(SendMessage);
            MessagePopupCommand = new Command(OpenPopupMessage);
            MessagingCenter.Subscribe<MessagePopupViewModel, Message>(this, MessengerKeys.NewMessage, OnMessageAdded);
            Message = "";
        }

        private async void EditMessage()
        {
            await PopupNavigation.PushAsync(new MessagePopup(new EditableMessagePopupViewModel(SelectedItem)));
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

        private async void DeleteMessage(string obj)
        {
            if (!await Application.Current.MainPage.DisplayAlert(
                title: "Confirmación",
                message: "¿Estás seguro de eliminar el mensaje?",
                accept: "Sí",
                cancel: "No")) return;
            foreach (var message in Messages.ToList())
            {
                if (!message.Title.Equals(obj)) continue;
                Messages.Remove(message);
                new MessagesRepository().DeleteMessage(message);
            }
        }

        private void OnMessageAdded(MessagePopupViewModel arg1, Message message)
        {
            if (Messages.Contains(message))
                Messages = new ObservableCollection<Message>(Messages.ToList());
            else
                Messages.Add(message);
            new MessagesRepository().AddMessage(message);
        }

        private async void OpenPopupMessage()
        {
            await PopupNavigation.PushAsync(new MessagePopup(new NewMessagePopupViewModel()));
        }

        private void SendMessage()
        {
            MessagingCenter.Send(this, MessengerKeys.Message, Message);
        }
    }
}