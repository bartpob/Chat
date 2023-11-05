using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Connection.Datagrams
{
    [JsonDerivedType(typeof(DatagramBase), typeDiscriminator: "Base")]
    [JsonDerivedType(typeof(UserStateDatagram), typeDiscriminator: "UserState")]
    public class DatagramBase
    {

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };
        public byte[] Encode()
        {
            string Serialized = JsonSerializer.Serialize(this, _options);
            int len = Serialized.Length;
            List<byte> Bytes = BitConverter.GetBytes(len).ToList();
            Bytes.AddRange(Encoding.UTF8.GetBytes(Serialized));
            return Bytes.ToArray();
        }

        public static DatagramBase Decode(byte[] Data)
        {
            var Deserialized = JsonSerializer.Deserialize<DatagramBase>(Encoding.UTF8.GetString(Data.Skip(4).ToArray()));

            return Deserialized ?? throw new Exception("Error in data transmission");
        }
    }
}
