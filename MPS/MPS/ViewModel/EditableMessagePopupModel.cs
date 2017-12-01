using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPS.Model;

namespace MPS.ViewModel
{
    public class EditableMessagePopupModel : MessagePopupModel
    {
        public EditableMessagePopupModel(Message message)
        {
            PopupTitle = "Editar mensaje";
            Message = message;            
        }
    }
}
