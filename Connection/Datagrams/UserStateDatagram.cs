using Connection.JsonConverters;
using System.Net;
using System.Text.Json.Serialization;

namespace Connection.Datagrams
{

    public class UserStateDatagram : DatagramBase
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AllowingResponse AllowingResponse { get; set; }

        [JsonConverter(typeof(JsonStringIPAddressConverter))]
        public IPAddress IPAddr { get; set; }
        public string HostName { get; set; }
        public byte[] Modulus { get; set; }
        public byte[] Exponent { get; set; }

        public UserStateDatagram(UserStatus status, IPAddress iPAddr, string hostName, AllowingResponse allowingResponse, byte[] modulus, byte[] exponent)
        {
            Status = status;
            IPAddr = iPAddr;
            HostName = hostName;
            AllowingResponse = allowingResponse;
            Modulus = modulus;
            Exponent = exponent;
        }
    }

    public enum UserStatus
    {
        Online,
        Offline
    };
    
    public enum AllowingResponse
    {
        Allowed,
        NotAllowed
    };


}
