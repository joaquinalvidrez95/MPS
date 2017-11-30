using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MPS
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessagePopup : PopupPage
	{
		public MessagePopup (MessagePopupViewModel viewModel)
		{
			InitializeComponent ();
		    BindingContext = viewModel;
		}

	    protected override Task OnAppearingAnimationEnd()
	    {	        
	        return Content.FadeTo(1);
        }
	
	    protected override Task OnDisappearingAnimationBegin()
	    {
	        return Content.FadeTo(1);
	    }

	    protected override bool OnBackButtonPressed()
	    {	       
	        return true;
	    }
	   
	}

    
}