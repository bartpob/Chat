using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Generic;
using Connection.Datagrams;
using System.Security.Cryptography;

namespace Connection.UDP
{
    internal class UDPReceiver
    {
        public event EventHandler<ReceivedDataEventArgs>? ReceivedData;
        private readonly Socket _socket;
        private Thread? _listeningThread;
        private readonly RSAParameters _rsaParameters;
        public UDPReceiver(Socket socket, RSAParameters rsaParameters)
        {
            _socket = socket;
            StartListening();

            _rsaParameters = rsaParameters;
        }

        private void StartListening()
        {
            ThreadStart listener = new ThreadStart(Listen);
            _listeningThread = new Thread(listener);
            _listeningThread.IsBackground = true;
            _listeningThread.Start();
        }
        public void Listen()
        {
            int dataLength = 0;
            List<byte> bytes = new List<byte>();
            while (true)
            {
                
                byte[] buffer = new byte[_socket.ReceiveBufferSize];
                int len = _socket.Receive(buffer);
                if(len != 0 && dataLength == 0)
                {
                    dataLength = BitConverter.ToInt32(buffer.Take(4).ToArray(), 0);
                    buffer = buffer.Skip(4).Take(len - 4).ToArray();
                    len -= 4;
                }

                if (dataLength != 0)
                {
                    var x = buffer.Take(len);
                    bytes.AddRange(buffer.Take(len));
                    dataLength -= len;

                    if(dataLength == 0)
                    {
                        if(ReceivedData != null)
                        {
                            byte encrypted = bytes.ElementAt(0);
                            if(encrypted == 1)
                            {
                                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                                {
                                    rsa.ImportParameters(_rsaParameters);
                                    bytes = rsa.Decrypt(bytes.ToArray(), false).ToList();
                                }
                            }
                            var datagram = DatagramBase.Decode(bytes.Skip(1).ToArray());
                            ReceivedData(this, new(datagram));
                        }
                        bytes.Clear();
                    }
                }
            }
        }
    }
}
