using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Connection.UDP;

namespace Connection
{
    public class ReceivedDataEventArgs : EventArgs
    {
        private IUdpDatagram _datagram;

        public IUdpDatagram Datagram
        {
            get
            {
                return _datagram;
            }
        }

        public ReceivedDataEventArgs(IUdpDatagram datagram)
        {
            _datagram = datagram;
        }
    }
}
