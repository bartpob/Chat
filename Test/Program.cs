using Connection.UDP;
using System.Net.Sockets;
using System.Net;
using Connection;
using Connection.Datagrams;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;

UDPConnectionProvider cp = new();
MessageDispatcher md = new(cp);

md.ReceivedUserState += rec;
void rec(object? sender, ReceivedDataEventArgs e)
{
    if (e.Datagram is UserStateDatagram u)
    {
        Console.WriteLine($"{u.HostName}, {u.IPAddr}, {u.Status}");
    }
}


while (true) ;
