using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connection.UDP
{
    public interface IUdpDatagram
    {
        byte[] Encode();

        IUdpDatagram Decode(byte[] data);
    }
}
