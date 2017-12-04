using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MessagePageModel : BaseViewModel
    {
        private string _message;
        private ObservableCollection<Message> _messages;

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
        public MessagePageModel()
        {
            ItemTappedCommand = new Command(EditMessage);
            Messages = new ObservableCollection<Message>(new MessagesRepository().Messages.ToList());
            DeleteCommand = new Command<Message>(DeleteMessage);
            SendCommand = new Command<Message>(SendMessage);
            MessagePopupCommand = new Command(OpenPopupMessage);
            MessagingCenter.Subscribe<MessagePopupModel, Message>(this, MessengerKeys.NewMessage, OnMessageAdded);
            Message = "";
        }

        private void SendMessage(Message selectedMessage)
        {
            MessagingCenter.Send(this, MessengerKeys.Message, selectedMessage.Text);
        }

        private async void DeleteMessage(Message messageSelected)
        {
            if (!await Application.Current.MainPage.DisplayAlert(
                title: "Confirmación",
                message: "¿Estás seguro de eliminar el mensaje?",
                accept: "Sí",
                cancel: "No")) return;

            new MessagesRepository().DeleteMessage(messageSelected);
            Messages = new ObservableCollection<Message>(new MessagesRepository().Messages);

        }

        private async void EditMessage()
        {
            await PopupNavigation.PushAsync(new MessagePopup(new EditableMessagePopupModel(SelectedItem)));
        }

        //private void SendMessage(int id)
        //{
        //    foreach (var message in Messages.ToList())
        //    {
        //        if (!message.Id.Equals(id)) continue;
        //        MessagingCenter.Send(this, MessengerKeys.Message, message.Text);
        //        break;
        //    }
        //}

        //private async void DeleteMessage(int id)
        //{
        //    if (!await Application.Current.MainPage.DisplayAlert(
        //        title: "Confirmación",
        //        message: "¿Estás seguro de eliminar el mensaje?",
        //        accept: "Sí",
        //        cancel: "No")) return;
        //    foreach (var message in Messages.ToList())
        //    {
        //        if (!message.Id.Equals(id)) continue;
        //        //Messages.Remove(message);
        //        new MessagesRepository().DeleteMessage(message);
        //        Messages=new ObservableCollection<Message>(new MessagesRepository().Messages);
        //    }
        //}

        private void OnMessageAdded(MessagePopupModel arg1, Message message)
        {
            if (Messages.Contains(message))
                Messages = new ObservableCollection<Message>(Messages.ToList());
            else
                Messages.Add(message);
            new MessagesRepository().AddMessage(message);
        }

        private async void OpenPopupMessage()
        {
            await PopupNavigation.PushAsync(new MessagePopup(new NewMessagePopupModel()));
        }


    }
}