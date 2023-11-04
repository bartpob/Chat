using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Connection.Datagrams;
using Connection.UDP;

namespace Connection
{
    public class ReceivedDataEventArgs : EventArgs
    {
       private DatagramBase _datagram;

        public DatagramBase Datagram
        {
            get
            {
                return _datagram;
            }
        }

        public ReceivedDataEventArgs(DatagramBase datagram)
        {
            _datagram = datagram;
        }
    }
}
