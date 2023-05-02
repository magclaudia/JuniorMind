using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Stream
{
    public class Program
    {
        public static byte[] WriteToStream(MemoryStream stream, string inputText, bool gzipped, bool encrypted)
        {
            if (gzipped)
            {
                using var gzipStream = new GZipStream(stream, CompressionMode.Compress, true);
                using var writer = new StreamWriter(gzipStream, Encoding.UTF8);
                writer.Write(inputText);
            }

            if (encrypted)
            {
                using var encrypt = Aes.Create();
                encrypt.GenerateKey();
                encrypt.GenerateIV();
                ICryptoTransform encryptor = encrypt.CreateEncryptor(encrypt.Key, encrypt.IV);
                using CryptoStream csEncrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write, true);
                using var wrEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8);
                wrEncrypt.Write(inputText);
            }

            byte[] array = stream.ToArray();
            return array;
        }

        public static string ReadFromStream(byte[] initialData, string inputText, bool gzipped, bool encrypted)
        {
            using var ms = new MemoryStream(initialData);
            if (gzipped)
            {
                using var gzipStream = new GZipStream(ms, CompressionMode.Decompress, true);
                using var reader = new StreamReader(gzipStream);
                reader.ReadToEnd();
            }

            if (encrypted)
            {
                using var aes = Aes.Create();
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read, true);
                using var reader = new StreamReader(cryptoStream);
                reader.ReadToEnd();
            }

            return inputText;
        }
    }
}