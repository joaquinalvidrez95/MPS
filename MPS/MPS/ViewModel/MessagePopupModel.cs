using System.Collections.Generic;
using System.Windows.Input;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public abstract class MessagePopupModel : BaseViewModel
    {
        private string _popupTitle;
        protected Message Message;
        private bool _isErrorMessageVisible;

        public string Title
        {
            get => Message.Title;
            set => Message.Title = value;
        }

        public bool IsErrorMessageVisible
        {
            get => _isErrorMessageVisible;
            set { _isErrorMessageVisible = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get => Message.Text;
            set => Message.Text = value;
        }
        public string PopupTitle
        {
            get => _popupTitle;
            protected set
            {
                _popupTitle = value;
                OnPropertyChanged();
            }
        }

        public ICommand DoneCommand { get; protected set; }


        public ICommand CancelCommand { get; protected set; }
        public ICollection<Message> Messages { get; }

        protected MessagePopupModel(ICollection<Message> messages)
        {
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
            Message = new Message();
            Messages = messages;
        }

        private async void CancelMessage()
        {
            await PopupNavigation.PopAsync();
        }

        private async void FinishMessage()
        {
            IsErrorMessageVisible = false;
            foreach (var message in Messages)
            {
                if (message.Title != Title) continue;
                IsErrorMessageVisible = true;
                break;
            }           
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Text) || IsErrorMessageVisible) return;
            await PopupNavigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.NewMessage, Message);
        }
    }
}