using Connection.Datagrams;
using Connection.UDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
    public class MessageDispatcher
    {
        private readonly UDPConnectionProvider _connectionProvider;
        private readonly RSAParameters _rsaParameters;
        public EventHandler<ReceivedDataEventArgs>? ReceivedUserState;
        public EventHandler<ReceivedDataEventArgs>? ReceivedMessage;

        public IPAddress IPAddr
        {
            get
            {
                return _connectionProvider.LocalIPAddress;
            }
        }
        public MessageDispatcher(UDPConnectionProvider connectionProvider, RSAParameters rsaParameters)
        {
            _connectionProvider = connectionProvider;

            _connectionProvider.ReceivedData += ReceivedDatagramEventHandler;

            _rsaParameters = rsaParameters;
        }

        public void Run()
        {
            SendState(UserStatus.Online);
        }

        public void Send(DatagramBase datagram, IPAddress? ipAddress = null, RSAParameters? publicKey = null)
        {
            _connectionProvider.Send(datagram, ipAddress, publicKey);
        }

        public void SendState(UserStatus status = UserStatus.Online, AllowingResponse allowingResponse = AllowingResponse.Allowed)
        {
            Send(new UserStateDatagram(status, _connectionProvider.LocalIPAddress, _connectionProvider.HostName, allowingResponse, _rsaParameters.Modulus!, _rsaParameters.Exponent!));
        }


        private void ReceivedDatagramEventHandler(object? sender, ReceivedDataEventArgs e)
        {
            if (e.Datagram is UserStateDatagram userState)
            {
                if (userState.Status != UserStatus.Offline && userState.AllowingResponse == AllowingResponse.Allowed)
                {
                    SendState(UserStatus.Online, AllowingResponse.NotAllowed);
                }
                if (ReceivedUserState != null)
                {
                    ReceivedUserState(this, e);
                }
            }

            if(e.Datagram is MessageDatagram messageDatagram)
            {
                if(ReceivedMessage != null)
                {
                    ReceivedMessage(this, e);
                }
            }
        }


    }
}
