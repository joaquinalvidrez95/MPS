using System.Windows.Input;

namespace MPS.ViewModel
{
    public abstract class IMessagePopupVideModel: BaseViewModel
    {
        private string _popupTitle;

        public string PopupTitle
        {
            get => _popupTitle;
            set
            {
                _popupTitle = value;
                OnPropertyChanged();
            }
        }

        public ICommand DoneCommand { get; protected set; }

        public string Title
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public ICommand CancelCommand { get; protected set; }
    }
}