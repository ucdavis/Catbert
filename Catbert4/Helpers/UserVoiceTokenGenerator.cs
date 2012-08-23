using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

// NB: For .NET your json string will look something like this:
//
// string userDetails = @"{""display_name"":""Richard White"",
// ""avatar_url"":""http:\/\/acme.com\/users\/1234\/avatar.png"",
// ""admin"":""accept"",
// ""expires"":""2009/05/14 03:38:31"",
// ""url"":""http:\/\/acme.com\/users\/1234"",
// ""email"":""rich@acme.com"",
// ""guid"":""1234""}";
namespace Catbert4.Helpers
{
    public class UserVoiceTokenGenerator
    {
        public static string Create(string displayName, string email, string guid, int minutesUntilExpiration = (60*24))
        {
            var date = DateTime.UtcNow.AddMinutes(minutesUntilExpiration);
            var datestring = date.ToString("yyyy/MM/dd H:mm:ss");

            var userDetails = new StringBuilder("{");

            userDetails.AppendFormat(
                @"""display_name"":""{0}"",
            ""expires"":""{1}"",
            ""email"":""{2}"",
            ""trusted"":""true"",
            ""guid"":""{3}""",
                displayName, datestring, email, guid);

            userDetails.Append("}");

            return Create(userDetails.ToString());
        }

        public static string Create(string userDetails)
        {
            string accountKey = "ucdavis";
            string apiKey = "b7bce8a7c70fb5eca0c1dc0fb4e01cc9";
            string initVector = "OpenSSL for Ruby"; // DO NOT CHANGE

            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] keyBytesLong;
            using (var sha = new SHA1CryptoServiceProvider())
            {
                keyBytesLong = sha.ComputeHash(Encoding.UTF8.GetBytes(apiKey + accountKey));
            }
            var keyBytes = new byte[16];
            Array.Copy(keyBytesLong, keyBytes, 16);

            byte[] textBytes = Encoding.UTF8.GetBytes(userDetails);
            for (int i = 0; i < 16; i++)
            {
                textBytes[i] ^= initVectorBytes[i];
            }

            // Encrypt the string to an array of bytes
            byte[] encrypted = encryptStringToBytes_AES(textBytes, keyBytes, initVectorBytes);
            string encoded = Convert.ToBase64String(encrypted);
            return HttpUtility.UrlEncode(encoded);
        }

        private static byte[] encryptStringToBytes_AES(byte[] textBytes, byte[] Key, byte[] IV)
        {
            // Declare the stream used to encrypt to an in memory
            // array of bytes and the RijndaelManaged object
            // used to encrypt the data.
            using (var msEncrypt = new MemoryStream())
            using (var aesAlg = new RijndaelManaged())
            {
                // Provide the RijndaelManaged object with the specified key and IV.
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.KeySize = 128;
                aesAlg.BlockSize = 128;
                aesAlg.Key = Key;
                aesAlg.IV = IV;
                // Create an encrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor();

                // Create the streams used for encryption.
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(textBytes, 0, textBytes.Length);
                    csEncrypt.FlushFinalBlock();
                }

                byte[] encrypted = msEncrypt.ToArray();
                // Return the encrypted bytes from the memory stream.
                return encrypted;
            }
        }
    }
}