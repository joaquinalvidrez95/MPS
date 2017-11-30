using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Model;

namespace MPS.ViewModel
{
    public class EditableMessagePopupViewModel : MessagePopupViewModel
    {
        public EditableMessagePopupViewModel(Message message)
        {
            PopupTitle = "Editar mensaje";
            Message = message;            
        }
    }
}
