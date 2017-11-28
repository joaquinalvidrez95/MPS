using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPS.Model
{
    public class Message
    {
        public Message(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public string Text { set; get; }
        public string Title { set; get; }
        public int id { set; get; }
    }
}
