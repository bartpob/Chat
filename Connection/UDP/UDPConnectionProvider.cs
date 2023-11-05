using Connection.Datagrams;
using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace Connection.UDP
{
    public class UDPConnectionProvider
    {
        private Socket _socket;
        private readonly IPAddress _groupIPAddress = IPAddress.Parse("239.255.255.255");

        private readonly int _port = 8181;
        private readonly int _sendBufferSize = 1024;
        private readonly int _receiveBufferSize = 1024;

        private readonly UDPSender _udpSender;
        private readonly UDPReceiver _udpReceiver;

        private readonly IPEndPoint _remoteEP;

        public readonly IPAddress _localIPAddress;
        public readonly string _hostName;

        public EventHandler<ReceivedDataEventArgs>? ReceivedData;
        public UDPConnectionProvider()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _remoteEP = new IPEndPoint(_groupIPAddress, _port);
            
            _socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, true);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, true);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(_groupIPAddress, _localIPAddress!));
            _socket.MulticastLoopback = false;
            _socket.Bind(new IPEndPoint(_localIPAddress!, _port));
            _socket.SendBufferSize = _sendBufferSize;
            _socket.ReceiveBufferSize = _receiveBufferSize;
            _udpSender = new UDPSender(_remoteEP, _socket);
            _udpReceiver = new UDPReceiver(_socket);
            _localIPAddress = IPAddress.Parse(GetLocalIPAddress());
            _hostName = GetHostName();

            _udpReceiver.ReceivedData += ReceivedDataEventHandler;
        }

        public void Send(DatagramBase datagram, IPAddress? ipAddress = null)
        {
            if (ipAddress == null)
            {
                _udpSender.SendMulticast(datagram);
            }
            else
            {
                _udpSender.Send(datagram, new IPEndPoint(ipAddress, _port));
            }
        }

        private void ReceivedDataEventHandler(object? sender, ReceivedDataEventArgs e)
        {
            if(ReceivedData != null)
            {
                ReceivedData(this, e);
            }
        }

        private string GetHostName()
        {
            return Dns.GetHostName() ?? "unavailable";
        }

        private string GetLocalIPAddress()
        {
            return Dns.GetHostEntry(GetHostName()).AddressList[0].ToString() ?? "0.0.0.0";
        }
        
    }
}
