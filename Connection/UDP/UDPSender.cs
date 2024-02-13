using System;
using System.Net;
using System.Net.Sockets;
using Connection.Datagrams;

namespace Connection.UDP
{
    internal class UDPSender
    {
        private readonly IPEndPoint _remoteEP;
        private readonly Socket _socket;


        public UDPSender(IPEndPoint remoteEP, Socket socket)
        {
            _remoteEP = remoteEP;
            _socket = socket;
        }
        public void Send(DatagramBase datagram, IPEndPoint ep)
        {
            byte[] bytesToSend = datagram.Encode();
            int leftLen = bytesToSend.Length;
            int sentLen = 0;

            while (leftLen != 0)
            {
                if (leftLen >= _socket.SendBufferSize)
                {
                    byte[] data = bytesToSend.Skip(sentLen).Take(_socket.SendBufferSize).ToArray();
                    _socket.SendTo(data, ep);
                    sentLen += _socket.SendBufferSize;
                    leftLen -= _socket.SendBufferSize;
                }
                else
                {
                    byte[] data = bytesToSend.Skip(sentLen).Take(leftLen).ToArray();
                    _socket.SendTo(data, ep);
                    leftLen = 0;
                }
            }
        }

        public void SendMulticast(DatagramBase datagram)
        {
            Send(datagram, _remoteEP);
        }
    }
}