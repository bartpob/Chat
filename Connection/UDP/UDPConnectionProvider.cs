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
        private readonly IPAddress _localIPAddress = IPAddress.Parse("192.168.3.4");

        private readonly int _port = 8181;
        private readonly int _sendBufferSize = 1024;
        private readonly int _receiveBufferSize = 1024;

        private readonly UDPSender _udpSender;
        private readonly UDPReceiver _udpReceiver;
        public UDPConnectionProvider()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            
            _socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, true);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, true);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(_groupIPAddress, _localIPAddress));
            _socket.MulticastLoopback = false;
            _socket.Bind(new IPEndPoint(_localIPAddress, _port));
            _socket.SendBufferSize = _sendBufferSize;
            _socket.ReceiveBufferSize = _receiveBufferSize;
            _udpSender = new UDPSender();
            _udpReceiver = new UDPReceiver();
        }

        public void Send(IUdpDatagram datagram)
        {
            _udpSender.Send(_socket, datagram, new IPEndPoint(_groupIPAddress,_port));
        }
        
    }
}
