using Connection.JsonConverters;
using System.Net;
using System.Text.Json.Serialization;

namespace Connection.Datagrams
{

    public class UserStateDatagram : DatagramBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringIPAddressConverter))]
        public IPAddress IPAddr { get; set; }
        public string HostName { get; set; }

        public UserStateDatagram(UserStatus status, IPAddress iPAddr, string hostName)
        {
            Status = status;
            IPAddr = iPAddr;
            HostName = hostName;
        }
    }

    public enum UserStatus
    {
        Online,
        Offline
    };


}
