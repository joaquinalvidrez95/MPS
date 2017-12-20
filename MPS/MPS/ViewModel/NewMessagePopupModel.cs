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

        protected override void ValidateTitle(string value)
        {
            foreach (var message in Messages)
            {
                if (message.Title == value)
                {
                    IsErrorMessageVisible = true;
                    break;
                }
                IsErrorMessageVisible = false;
            }
        }

    }
}