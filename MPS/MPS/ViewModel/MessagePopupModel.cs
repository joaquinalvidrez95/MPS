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
        //protected Message Message;
        private bool _isErrorMessageVisible;

        //private ICommand _textChangedCommand;

        //private string _title;

        public Message Message { get; set; }
        public string Title
        {
            get => Message.Title;
            set
            {
                //TextChangedCommand = null;
                Message.Title = value;
                //TextChangedCommand = new Command(OnTextChanged);
            }
        }

        public string Text
        {
            get => Message.Text;
            set
            {
              
                Message.Text = value;
            
            }
        }

        public ICommand TextChangedCommand { get; private set; }

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
            Message = new Message();
            Messages = messages;
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
            TextChangedCommand = new Command(OnTextChanged);
        }

        protected MessagePopupModel(ICollection<Message> messages, Message message)
        {
            Messages = messages;
            Message = message;
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
            TextChangedCommand = new Command(OnTextChanged);
        }

        private async void CancelMessage()
        {
            await PopupNavigation.PopAsync();
        }

        private async void FinishMessage()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Text) || IsErrorMessageVisible) return;
            await PopupNavigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.NewMessage, Message);
        }

        private void OnTextChanged()
        {
            foreach (var message in Messages)
            {
                if (message.Title == Title)
                {
                    IsErrorMessageVisible = true;
                    break;
                }
                IsErrorMessageVisible = false;
            }

        }
    }
}