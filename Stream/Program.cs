using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace StreamProj
{
    public class Program
    {
        private static Aes aes;
        static Program()
        {
            aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
        }

        public static void WriteToStream(Stream stream, string inputText, bool gzipped = false, bool encrypted = false)
        {
            var writer = new StreamWriter(stream);
            Stream cipherStream = stream;
            if (gzipped)
            {
               cipherStream = new GZipStream(stream, CompressionMode.Compress);
            }

            if (encrypted)
            {
                using ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                var crypto = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write);
            }

            writer = new StreamWriter(cipherStream);
            writer.Write(inputText);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
        }

        public static string ReadFromStream(Stream stream, bool gzipped = false, bool encrypted = false)
        {
            var reader = new StreamReader(stream);
            Stream decoded = stream;

            if (gzipped)
            {
                decoded = new GZipStream(stream, CompressionMode.Decompress);
            }

            if (encrypted)
            {
                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var crypto = new CryptoStream(decoded, decryptor, CryptoStreamMode.Read);
                crypto.FlushFinalBlock();
            }

            reader = new StreamReader(decoded);
            return reader.ReadToEnd();
        }
    }
}