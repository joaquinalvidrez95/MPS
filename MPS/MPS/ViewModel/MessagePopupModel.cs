using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public abstract class MessagePopupModel : BaseViewModel
    {
        private bool _isErrorMessageVisible;
        private readonly Message _messageToSent;
        private string _title;
        private int _leftCharacters;
        private string _text;

        public int LeftCharacters
        {
            get => _leftCharacters;
            set { _leftCharacters = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                foreach (var message in Messages)
                {
                    if (message.Title == value)
                    {
                        IsErrorMessageVisible = true;
                        break;
                    }
                    IsErrorMessageVisible = false;
                }
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                LeftCharacters = (int)Application.Current.Resources["MaxMessageLength"] - Text.Length;
            }
        }

        public bool IsErrorMessageVisible
        {
            get => _isErrorMessageVisible;
            set { _isErrorMessageVisible = value; OnPropertyChanged(); }
        }

        public string PopupTitle { get; protected set; }

        public ICommand DoneCommand { get; protected set; }

        public ICommand CancelCommand { get; protected set; }
        private ICollection<Message> Messages { get; }

        protected MessagePopupModel(ICollection<Message> messages)
        {
            _messageToSent = new Message();
            Text = "";
            Messages = messages;
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
        }

        protected MessagePopupModel(ICollection<Message> messages, Message message)
        {
            Messages = messages;
            _messageToSent = message;
            Title = message.Title;
            Text = message.Text;
            Messages.Remove(message);
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
        }

        private async void CancelMessage()
        {
            await PopupNavigation.PopAsync();
        }

        private async void FinishMessage()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Text) || IsErrorMessageVisible) return;
            await PopupNavigation.PopAsync();
            _messageToSent.Text = Text;
            _messageToSent.Title = Title;
            MessagingCenter.Send(this, MessengerKeys.NewMessage, _messageToSent);
        }

    }
}