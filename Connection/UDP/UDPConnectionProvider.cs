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
        private readonly RSAParameters _rsaParameters;

        private readonly int _port = 8181;
        private readonly int _sendBufferSize = 1024;
        private readonly int _receiveBufferSize = 1024;

        private readonly UDPSender _udpSender;
        private readonly UDPReceiver _udpReceiver;

        private readonly IPEndPoint _remoteEP;

        public readonly IPAddress LocalIPAddress;
        public readonly string HostName;

        public EventHandler<ReceivedDataEventArgs>? ReceivedData;
        public UDPConnectionProvider(RSAParameters rsaParameters)
        {
            LocalIPAddress = IPAddress.Parse(GetLocalIPAddress());
            HostName = GetHostName();

            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            _remoteEP = new IPEndPoint(_groupIPAddress, _port);
            _rsaParameters = rsaParameters;
            _socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, true);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, true);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(_groupIPAddress, LocalIPAddress!));
            _socket.MulticastLoopback = false;
            _socket.Bind(new IPEndPoint(LocalIPAddress!, _port));
            _socket.SendBufferSize = _sendBufferSize;
            _socket.ReceiveBufferSize = _receiveBufferSize;
            _udpSender = new UDPSender(_remoteEP, _socket, _rsaParameters);
            _udpReceiver = new UDPReceiver(_socket, _rsaParameters);

            _udpReceiver.ReceivedData += ReceivedDataEventHandler;
        }

        public void Send(DatagramBase datagram, IPAddress? ipAddress = null, RSAParameters? publicKey = null)
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
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        
    }
}
