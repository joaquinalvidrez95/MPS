using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Model
{
    public class MessagesRepository
    {
        public IList<Message> Messages { get; set; }

        public MessagesRepository()
        {         
            Task.Run(async () =>
                Messages = await App.Database.GetMessagesAsync()).Wait();
        }

        public void AddMessage(Message message)
        {
            App.Database.SaveMessageAsync(message);
        }

        public void DeleteMessage(Message message)
        {
            App.Database.DeleteMessageAsync(message);
        }
    }
}
