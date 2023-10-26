using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Connection.UDP
{
    public class UDPReceiver
    {
        public void Listen(Socket socket)
        {
            while(true)
            {
                byte[] buffer = new byte[1024];
                int len = socket.Receive(buffer);
                Console.WriteLine(len);
            }
        }
    }
}
