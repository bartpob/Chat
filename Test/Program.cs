using Connection.UDP;
using System.Net.Sockets;
using System.Net;
using Connection;

UDPConnectionProvider uDPConnectionProvider = new UDPConnectionProvider();

uDPConnectionProvider.ReceivedData += receive;
void receive(object? sender, ReceivedDataEventArgs e)
{
    SimpleDatagram dg = (SimpleDatagram)e.Datagram;
    Console.WriteLine(dg.Text);
}


while (true);

