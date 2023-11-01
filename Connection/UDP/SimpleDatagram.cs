using Connection.UDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Connection.UDP
{
    public class SimpleDatagram : IUdpDatagram
    {
        public string Text { get; set; }

        public SimpleDatagram(string text)
        {
            Text = text;
        }
        public static IUdpDatagram Decode(byte[] data)
        {
            var deserialized = JsonSerializer.Deserialize<SimpleDatagram>(System.Text.Encoding.UTF8.GetString(data));

            return deserialized ?? throw new Exception("Couldn't deserialize datagram");
        }

        public byte[] Encode()
        {
            throw new NotImplementedException();
        }
    }
}
