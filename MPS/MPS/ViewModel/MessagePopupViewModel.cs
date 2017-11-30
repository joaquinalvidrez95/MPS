using System.Windows.Input;
using MPS.Model;
using MPS.Utilities;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS.ViewModel
{
    public abstract class MessagePopupViewModel : BaseViewModel
    {
        private string _popupTitle;
        protected Message Message;

        public string Title
        {
            get => Message.Title;
            set => Message.Title = value;
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

        protected MessagePopupViewModel()
        {
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
            Message = new Message();
        }        

        private async void CancelMessage()
        {
            await PopupNavigation.PopAsync();
        }

        private async void FinishMessage()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Text) || Title.Length >= 199) return;
            await PopupNavigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.NewMessage, Message);
        }
    }
}