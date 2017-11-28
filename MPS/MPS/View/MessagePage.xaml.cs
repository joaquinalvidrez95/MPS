using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MPS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessagePage : ContentPage
    {
        public MessagePage()
        {
            InitializeComponent();         
        }
             
        private async void OnOpenPupup(object sender, EventArgs e)
        {
            var page = new MessagePopup();

            //await Navigation.PushPopupAsync(page);

            await PopupNavigation.PushAsync(page);
        }


    }
}