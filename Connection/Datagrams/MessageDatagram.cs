using Connection.JsonConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Connection.Datagrams
{
    public class MessageDatagram : DatagramBase
    {
        [JsonConverter(typeof(JsonStringIPAddressConverter))]
        public IPAddress FromIPAddr { get; set; }
        public string Text { get; set; }

        [JsonConverter(typeof(JsonStringDateTimeConverter))]
        public DateTime Date { get; set; }


        public MessageDatagram(IPAddress from, string text, DateTime date)
        {
            FromIPAddr = from;
            Text = text;
            Date = date;
        }
    }
}
