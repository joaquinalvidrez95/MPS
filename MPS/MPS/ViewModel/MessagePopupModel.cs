using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using MPS.Helper;
using MPS.Model;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public abstract class MessagePopupModel : BaseViewModel
    {
        private bool _isErrorMessageVisible;
        protected readonly Message MessageToSent;
        private string _title;
        private string _text;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                ValidateTitle(value);
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
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
        protected ICollection<Message> Messages { get; }

        protected MessagePopupModel(ICollection<Message> messages)
        {
            MessageToSent = new Message();
            Messages = messages;
            Text = "";
            Title = "";
            DoneCommand = new Command(FinishMessage, () => false);
            CancelCommand = new Command(CancelMessage);
        }

        protected MessagePopupModel(ICollection<Message> messages, Message message)
        {
            Messages = messages;
            MessageToSent = message;
            Title = message.Title;
            Text = message.Text;
            DoneCommand = new Command(FinishMessage, () => false);
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
            MessageToSent.Text = Text;
            MessageToSent.Title = Title;         
            MessagingCenter.Send(this, MessengerKeys.Message, MessageToSent);
        }

        protected abstract void ValidateTitle(string value);

    }
}