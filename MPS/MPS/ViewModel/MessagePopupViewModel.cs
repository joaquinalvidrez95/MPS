using System.Windows.Input;
using MPS.Model;
using MPS.Utilities;
using MPS.ViewModel;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MPS
{
    public class MessagePopupViewModel : IMessagePopupVideModel
    {        

        public MessagePopupViewModel()
        {
            DoneCommand = new Command(FinishMessage);
            CancelCommand = new Command(CancelMessage);
            PopupTitle = "Agregar Mensaje";
        }

        private async void CancelMessage()
        {
            await PopupNavigation.PopAsync();
        }

        private async void FinishMessage()
        {
            if (string.IsNullOrEmpty(Title) || string.IsNullOrEmpty(Text) || Title.Length >= 199) return;         
            await PopupNavigation.PopAsync();
            MessagingCenter.Send(this, MessengerKeys.Message2, new Message(Title, Text));
        }

    }
}