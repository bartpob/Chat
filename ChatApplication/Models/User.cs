using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Connection.Datagrams;

namespace ChatApplication.Models
{
    class User
    {
        public string Name { get; set; }
        public IPAddress Address { get; set; }
        public UserStatus Status { get; set; }

        public List<Message>? Messages { get; set; }
        public User(string name, IPAddress address, List<Message>? messages,  UserStatus status = UserStatus.Online)
        {
            Name = name;
            Address = address;
            Status = status;
            Messages = messages;
        }
        
    }
}
