using Connection.UDP;
using System.Net.Sockets;
using System.Net;
using Connection;
using Connection.Datagrams;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

MessageDatagram x = new MessageDatagram(IPAddress.Parse("127.1.1.1"), "witam", DateTime.Now);
var enc = x.Encode();
var dec = DatagramBase.Decode(enc.Skip(4).ToArray());


while (true) ;
