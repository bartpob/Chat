using Connection.UDP;
using System.Net.Sockets;
using System.Net;
using Connection;
using Connection.Datagrams;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Security.Cryptography;
using System.Text.Json;
using System.Net.NetworkInformation;


RSACryptoServiceProvider prow = new();

UnicodeEncoding byteconv = new();

byte[] dataToEncrypt = byteconv.GetBytes("Data to Encrypt");
byte[] encryptedData;
byte[] decryptedData;


using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
{
    var x = rsa.ExportParameters(false);

    UserStateDatagram dupka = new(UserStatus.Online, IPAddress.Parse("123.123.123"), "pizdeusz", AllowingResponse.NotAllowed, x.Modulus!, x.Exponent!);
    JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };
    string serialized = JsonSerializer.Serialize(dupka, _options);
    Console.WriteLine(serialized);

    var deserialized = JsonSerializer.Deserialize<UserStateDatagram>(serialized);
}
