using Connection.UDP;
using System.Net.Sockets;
using System.Net;
using Connection;
using Connection.Datagrams;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

UserStateDatagram dg = new(UserStatus.Online, IPAddress.Parse("129.12.12.12"), "dupkasz");

var x = dg.Encode();

var y = Encoding.UTF8.GetString(x.Skip(4).ToArray());

UserStateDatagram b = (UserStateDatagram)DatagramBase.Decode(x);
Console.WriteLine("xx");

