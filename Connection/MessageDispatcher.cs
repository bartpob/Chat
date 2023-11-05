﻿using Connection.Datagrams;
using Connection.UDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Connection
{
    public class MessageDispatcher
    {
        private readonly UDPConnectionProvider _connectionProvider;

        public EventHandler<ReceivedDataEventArgs>? ReceivedUserState;

        public MessageDispatcher(UDPConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;

            _connectionProvider.ReceivedData += ReceivedDatagramEventHandler;


            SendState(UserStatus.Online);
        }

        public void Send(DatagramBase datagram, IPAddress? ipAddress = null)
        {
            _connectionProvider.Send(datagram, ipAddress);
        }

        public void SendState(UserStatus status = UserStatus.Online)
        {
            Send(new UserStateDatagram(status, _connectionProvider.LocalIPAddress, _connectionProvider.HostName));
        }


        private void ReceivedDatagramEventHandler(object? sender, ReceivedDataEventArgs e)
        {
            if (e.Datagram is UserStateDatagram userState)
            {
                if(userState.Status != UserStatus.Offline)
                {
                    SendState();
                }
                if (ReceivedUserState != null)
                {
                    ReceivedUserState(this, e);
                }
            }
        }


    }
}