using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Connection.Datagrams;

namespace Connection.UDP
{
    internal class UDPSender
    {
        private readonly IPEndPoint _remoteEP;
        private readonly Socket _socket;
        private readonly RSAParameters _rsaParameters;

        public UDPSender(IPEndPoint remoteEP, Socket socket, RSAParameters rsaParameters)
        {
            _remoteEP = remoteEP;
            _socket = socket;
            _rsaParameters = rsaParameters;
        }
        public void Send(DatagramBase datagram, IPEndPoint ep, RSAParameters? publicKey = null)
        {
            var bytesToSend = datagram.Encode();
            if (publicKey != null)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(publicKey ?? throw new Exception("publicKey is null"));
                    bytesToSend = rsa.Encrypt(bytesToSend, false);
                }
            }


            List<byte> bytes = bytesToSend.ToList();
            bytes.Insert(0, (byte)bytes.Count);
            bytesToSend = bytes.ToArray();


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
                    break;
                }
            }
        }

        public void SendMulticast(DatagramBase datagram)
        {
            Send(datagram, _remoteEP);
        }
    }
}