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
        public EditableMessagePopupModel(Message message, ICollection<Message> messages) : base(messages, message)
        {
            PopupTitle = "Editar mensaje";
            //Message = message;            
        }


        protected override void ValidateTitle(string value)
        {
            foreach (var message in Messages)
            {
                if (message.Title == value && message.Title != MessageToSent.Title)
                {
                    IsErrorMessageVisible = true;
                    break;
                }
                IsErrorMessageVisible = false;
            }
        }
    }
}
