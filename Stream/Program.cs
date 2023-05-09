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

        public static void WriteToStream(Stream stream, string inputText, bool gzipped = false, bool encrypted = false)
        {
            Stream cipherStream = stream;
            if (gzipped)
            {
                cipherStream = new GZipStream(stream, CompressionMode.Compress);
            }

            if (encrypted)
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                cipherStream = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write);
            }

            var writer = new StreamWriter(cipherStream);
            writer.Write(inputText);
            writer.Flush();
            if (encrypted) 
            {
                ((CryptoStream)cipherStream).FlushFinalBlock();
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