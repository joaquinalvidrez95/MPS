using MPS.ViewModel;
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
    public partial class MainParametersPage : ContentPage
    {      

        public MainParametersPage()
        {
            InitializeComponent();
            BindingContext = new MainParametersViewModel(Navigation);
        }
       


    }
}