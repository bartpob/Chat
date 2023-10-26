using System;
using System.Net;
using System.Net.Sockets;


namespace Connection.UDP
{
    internal class UDPSender
    {
        public void Send(Socket socket, IUdpDatagram datagram, IPEndPoint EPRemote)
        {
            byte[] bytesToSend = datagram.Encode();
            int leftLen = bytesToSend.Length;
            int sentLen = 0;

            while (leftLen != 0)
            {
                if (leftLen >= socket.SendBufferSize)
                {
                    byte[] data = bytesToSend.Skip(sentLen).Take(socket.SendBufferSize).ToArray();
                    socket.SendTo(data, EPRemote);
                    sentLen += socket.SendBufferSize;
                    leftLen -= socket.SendBufferSize;
                }
                else
                {
                    byte[] data = bytesToSend.Skip(sentLen).Take(leftLen).ToArray();
                    socket.SendTo(data, EPRemote);
                    leftLen = 0;
                }
            }
        }
    }
}