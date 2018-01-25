using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using MPS.Helper;
using MPS.Model;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public class MessagePageModel : BaseViewModel
    {
        private string _message;
        private double _speed;

        private ObservableCollection<Message> _messages;
        private Message _selectedItem;


        public ICommand ItemTappedCommand { get; }

        public double Speed
        {
            get => _speed;
            set
            {
                //value = Math.Round(value / STEP_VALUE);
                //value = value * STEP_VALUE;

                value = Math.Round(value);
                if ((int)value != (int)_speed)
                {
                    MessagingCenter.Send(this, MessengerKeys.Speed, (int)value);
                }
                _speed = value;
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddMessageCommand { get; }

        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set { _messages = value; OnPropertyChanged(); }
        }

        public ICommand DeleteCommand { get; }

        public Message SelectedItem
        {
            //get => _selectedItem;
            get => null;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public ICommand SendCommand { get; }
        public MessagePageModel()
        {
            ItemTappedCommand = new Command(EditMessage);
            Messages = new ObservableCollection<Message>(new MessagesRepository().GetMessagesSorted());
            DeleteCommand = new Command<Message>(DeleteMessage);
            SendCommand = new Command<Message>(SendMessage);
            AddMessageCommand = new Command(OpenNewPopupMessage);
            Message = "";
            Speed = 3;
        }

        private void SendMessage(Message selectedMessage)
        {
            MessagingCenter.Send(this, MessengerKeys.Message, selectedMessage);
        }

        private async void DeleteMessage(Message messageSelected)
        {
            if (!await Application.Current.MainPage.DisplayAlert(
                (string)Application.Current.Resources["DisplayAlertTitleDeleteMessage"],
                (string)Application.Current.Resources["DisplayAlertMessageMessageDeletedQuestion"],
                (string)Application.Current.Resources["DisplayAlertAcceptDeleteMessage"],
                (string)Application.Current.Resources["DisplayAlertCancelNo"]
                )) return;

            new MessagesRepository().DeleteMessage(messageSelected);
            Messages = new ObservableCollection<Message>(new MessagesRepository().GetMessagesSorted());
            
        }

        private async void EditMessage()
        {
            await PopupNavigation.PushAsync(new MessagePopup(new EditableMessagePopupModel(_selectedItem, Messages)));
        }

        private void OnMessageAdded(MessagePopupModel arg1, Message message)
        {
            message.Text = message.Text.TrimEnd();
            if (Messages.Contains(message))
                //Messages = new ObservableCollection<Message>(Messages.ToList());
                SortMessagesByTitle();
            else
            {
                Messages.Add(message);
                SortMessagesByTitle();
            }

            new MessagesRepository().AddMessage(message);
        }

        private async void OpenNewPopupMessage()
        {
            await PopupNavigation.PushAsync(new MessagePopup(new NewMessagePopupModel(Messages)));
        }


        protected override void Subscribe()
        {
            MessagingCenter.Subscribe<MessagePopupModel, Message>(this, MessengerKeys.Message, OnMessageAdded);
            //MessagingCenter.Subscribe<MainPageModel, int>(this, MessengerKeys.Speed, OnSpeedReceived);
            MessagingCenter.Subscribe<Feedbacker, int>(this, MessengerKeys.Speed, OnSpeedReceived);
        }

        private void OnSpeedReceived(Feedbacker feedbacker, int i)
        {
            Speed = i % ((double)Application.Current.Resources["MaxSliderSpeed"] + 1);
        }

        private void SortMessagesByTitle()
        {
            var y = from element in Messages
                    orderby element.Title
                    select element;
            Messages = new ObservableCollection<Message>(y);
        }
    }
}