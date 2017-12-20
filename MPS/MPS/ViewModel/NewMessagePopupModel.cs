using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MPS.Model;

namespace MPS.ViewModel
{
    public class NewMessagePopupModel : MessagePopupModel
    {        

        public NewMessagePopupModel(ICollection<Message> messages) : base(messages)
        {            
            PopupTitle = "Agregar Mensaje";            
        }

  
    }
}