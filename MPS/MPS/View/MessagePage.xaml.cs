using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
       

        private void buttonMessageNew_Clicked(object sender, EventArgs e)
        {
            
            
            //var promptConfig = new PromptConfig();
            //promptConfig.InputType = InputType.Name;
            //promptConfig.IsCancellable = true;
            
            //promptConfig.Message = "Write your name";
            //var result = await UserDialogs.Instance.PromptAsync(promptConfig);
            //if (result.Ok)
            //{
            //    PromptedTextLabel.Text = result.Text;
            //}
        }

        
    }
}