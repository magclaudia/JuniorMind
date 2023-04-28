using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace ReadAndWriteProject
{
    class Program
    {
        static void Main(string[] args) 
        {
            bool gzipped = true;
            bool encrypted = true;
            using MemoryStream stream = new();
            WriteToStream(stream, gzipped, encrypted);
            stream.Seek(0, SeekOrigin.Begin);
            var result = ReadFromStream(stream, gzipped, encrypted);
            Console.WriteLine(result);
        }

        public static void WriteToStream(MemoryStream stream, bool gzipped, bool encrypted)
        {
            byte[] buffer = Encoding.UTF8.GetBytes("input text");
            if (gzipped)
            {
                using var outputStream = new MemoryStream();
                using var gzipStream = new GZipStream(outputStream, CompressionMode.Compress);
                gzipStream.Write(buffer, 0, buffer.Length);
                outputStream.ToArray();
            }
             
            if (encrypted)
            {
                using var encrypt = Aes.Create();
                encrypt.GenerateKey();
                encrypt.GenerateIV();
                ICryptoTransform encryptor = encrypt.CreateEncryptor(encrypt.Key, encrypt.IV);
                using MemoryStream msEncrypt = new MemoryStream();
                using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                csEncrypt.Write(buffer, 0, buffer.Length);
                buffer = msEncrypt.ToArray();
            }
        }

        public static string ReadFromStream(MemoryStream stream, bool gzipped, bool encrypted)
        {
            byte[] buffer = Encoding.UTF8.GetBytes("input text");

            if (gzipped)
            {
                using var inputStream = new MemoryStream(buffer);
                using var outputStream = new MemoryStream();
                using var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress);
                outputStream.ToArray();
            }

            if (encrypted)
            {
                using var aes = Aes.Create();
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var msDecrypt = new MemoryStream(buffer);
                using var cryptoStream = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var reader = new StreamReader(cryptoStream);
            }

            return Encoding.UTF8.GetString(buffer);
        }
    }
}