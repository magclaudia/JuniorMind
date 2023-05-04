using System.Formats.Asn1;
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
                cipherStream = new GZipStream(cipherStream, CompressionMode.Compress, true);
            }

            if (encrypted)
            {
                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                cipherStream = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write, true);
            }

            writer = new StreamWriter(cipherStream);
            writer.Write(inputText);
            writer.Flush();
        }

        public static string ReadFromStream(Stream stream, bool gzipped = false, bool encrypted = false)
        {
            Stream decoded = stream;

            if (gzipped)
            {
                decoded = new GZipStream(decoded, CompressionMode.Decompress);
            }

            if (encrypted)
            {
                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                decoded = new CryptoStream(stream, decryptor, CryptoStreamMode.Read);
            }


            var reader = new StreamReader(decoded);
            return reader.ReadToEnd();
        }

    }
}  