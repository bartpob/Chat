using Connection.UDP;
using System.Net.Sockets;
using System.Net;

 IPAddress _groupIPAddress = IPAddress.Parse("239.255.255.255");
 IPAddress _localIPAddress = IPAddress.Parse("192.168.3.4");

 int _port = 8181;
 int _sendBufferSize = 1024;
 int _receiveBufferSize = 1024;

Socket _socket;
_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

_socket.SetSocketOption(SocketOptionLevel.Udp, SocketOptionName.NoDelay, true);
_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, true);
_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(_groupIPAddress, _localIPAddress));
_socket.MulticastLoopback = false;
_socket.Bind(new IPEndPoint(_localIPAddress, _port));
_socket.SendBufferSize = _sendBufferSize;
_socket.ReceiveBufferSize = _receiveBufferSize;
