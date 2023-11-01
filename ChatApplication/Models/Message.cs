using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Models
{
    class Message
    {
        public string Text { get; set; }
        public MessageType Type { get; set; }

        public DateTime Date { get; set; }

        public Message(string text, MessageType type, DateTime date)
        {
            Type = type;
            Text = text;
            Date = date;
        }
    }

    enum MessageType
    { 
        Incoming,
        Outgoing
    };
}
