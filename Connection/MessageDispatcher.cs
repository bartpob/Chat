using Connection.Datagrams;
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

        private EventHandler<ReceivedDataEventArgs>? ReceivedUserState;

        public MessageDispatcher(UDPConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;

            _connectionProvider.ReceivedData += ReceivedDatagramEventHandler;
        }

        public void Send(DatagramBase datagram, IPAddress? ipAddress = null)
        {
            _connectionProvider.Send(datagram, ipAddress);
        }


        private void ReceivedDatagramEventHandler(object? sender, ReceivedDataEventArgs e)
        {
            if (e.Datagram is UserStateDatagram userState)
            {
                if (ReceivedUserState != null)
                {
                    ReceivedUserState(this, e);
                }
            }
        }


    }
}
