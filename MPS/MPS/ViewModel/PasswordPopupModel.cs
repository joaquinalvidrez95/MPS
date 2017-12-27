using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MPS.ViewModel
{
    public class PasswordPopupModel : BaseViewModel
    {
        private string _password;

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged();}
        }

        public ICommand DoneCommand { get; }

        public ICommand CancelCommand { get; }

        public PasswordPopupModel()
        {
            Password = "";
        }
    }
}
