using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.Generic;

namespace Connection.UDP
{
    internal class UDPReceiver
    {
        public event EventHandler<ReceivedDataEventArgs>? OnReceivedData;
        private readonly Socket _socket;
        private Thread? _listeningThread;
        public UDPReceiver(Socket socket)
        {
            _socket = socket;
            StartListening();
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
                        SimpleDatagram receivedDatagram = (SimpleDatagram)SimpleDatagram.Decode(bytes.ToArray());
                        ReceivedDataEventArgs eventArgs = new(receivedDatagram);
                        if (OnReceivedData != null)
                        {
                            OnReceivedData(this, eventArgs);
                        }
                        bytes.Clear();
                    }
                }
            }
        }
    }
}
