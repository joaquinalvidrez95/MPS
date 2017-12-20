using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        protected readonly Message MessageToSent;
        private string _title;
        private int _leftCharacters;
        private string _text;

        public int LeftCharacters
        {
            get => _text == null ? (int)Application.Current.Resources["MaxMessageLength"] : _leftCharacters;
            set { _leftCharacters = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                ValidateTitle(value);
            }
        }


        public string Text
        {
            get => _text;
            set
            {            
                _text = value;
                LeftCharacters = (int)Application.Current.Resources["MaxMessageLength"] - Text.Length;
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
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
        }

        protected MessagePopupModel(ICollection<Message> messages, Message message)
        {
            Messages = messages;
            MessageToSent = message;
            Title = message.Title;
            Text = message.Text;
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
            MessageToSent.Text = Text;
            MessageToSent.Title = Title;
            MessagingCenter.Send(this, MessengerKeys.NewMessage, MessageToSent);
        }

        protected abstract void ValidateTitle(string value);
    }
}