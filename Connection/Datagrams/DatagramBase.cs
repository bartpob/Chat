﻿using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Connection.Datagrams
{
    [JsonDerivedType(typeof(DatagramBase), typeDiscriminator: "Base")]
    [JsonDerivedType(typeof(UserStateDatagram), typeDiscriminator: "UserState")]
    public class DatagramBase
    {

        private JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };
        public byte[] Encode()
        {
            string serialized = JsonSerializer.Serialize(this, _options);
            int len = serialized.Length;
            List<byte> bytes = BitConverter.GetBytes(len).ToList();
            bytes.AddRange(Encoding.UTF8.GetBytes(serialized));
            return bytes.ToArray();
        }

        public static DatagramBase Decode(byte[] data)
        {
            var deserialized = JsonSerializer.Deserialize<DatagramBase>(Encoding.UTF8.GetString(data.Skip(4).ToArray()));

            return deserialized ?? throw new Exception("Error in data transmission");
        }
    }
}