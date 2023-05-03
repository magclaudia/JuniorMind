using System.Formats.Asn1;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace StreamProj
{
    public class Program
    {
        public static void WriteToStream(MemoryStream inputStream, string inputText, bool gzipped, bool encrypted)
        {
            var writer = new StreamWriter(inputStream);
            Stream cipherStream = inputStream;
            if (gzipped)
            {
                cipherStream = new GZipStream(inputStream, CompressionMode.Compress, true);
            }

            if (encrypted)
            {
                using var aes = Aes.Create();
                aes.GenerateKey();
                aes.GenerateIV();
                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                cipherStream = new CryptoStream(cipherStream, encryptor, CryptoStreamMode.Write, true);
            }

            writer = new StreamWriter(cipherStream);
            writer.Write(inputText);
            writer.Flush();
            //return writer.;
        }

        public static string ReadFromStream(MemoryStream initialData, string inputText, bool gzipped, bool encrypted)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(inputText));
            Stream decoded = ms;

            if (gzipped)
            {
                decoded = new GZipStream(ms, CompressionMode.Decompress, true);
            }

            if (encrypted)
            {
                using var aes = Aes.Create();
                aes.GenerateKey();
                aes.GenerateIV();
                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                decoded = new CryptoStream(decoded, decryptor, CryptoStreamMode.Read, true);
            }

            using var reader = new StreamReader(decoded);
            return reader.ReadToEnd();
        }
    }
}  