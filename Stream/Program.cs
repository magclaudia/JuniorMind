using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace StreamProj
{
    public class Program
    {
        private static readonly Aes aes;
        static Program()
        {
            aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
        }

        public static void WriteToStream(Stream stream, string data, bool gzipped = false, bool encrypted = false)
        {
            Stream dataStream = stream;
            if (gzipped)
            {
                dataStream = new GZipStream(stream, CompressionMode.Compress);
            }

            if (encrypted)
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                dataStream = new CryptoStream(dataStream, encryptor, CryptoStreamMode.Write);
            }

            var writer = new StreamWriter(dataStream);
            writer.Write(data);
            writer.Flush();
           
            if (dataStream is CryptoStream cryptoStream) 
            {
                cryptoStream.FlushFinalBlock();
            }
        }

        public static string ReadFromStream(Stream stream, bool gzipped = false, bool encrypted = false)
        {
            Stream decoded = stream;
            if (gzipped)
            {
                decoded = new GZipStream(stream, CompressionMode.Decompress);
            }

            if (encrypted)
            {
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                decoded = new CryptoStream(decoded, decryptor, CryptoStreamMode.Read);
            }

            var reader = new StreamReader(decoded);
            string convertedString = reader.ReadToEnd();
            reader.Close();
            return convertedString;
        }
    }
}