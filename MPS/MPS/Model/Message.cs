using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MPS.Model
{
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int Id { set; get; }
        public Message(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public Message()
        {           
        }

        public string Text { set; get; }
        public string Title { set; get; }
       
    }
}
