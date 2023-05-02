using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace StreamProj
{
    public class Program
    {
        public static byte[] WriteToStream(MemoryStream inputStream, string inputText, bool gzipped, bool encrypted)
        {
            var writer = new StreamWriter(inputStream);
            Stream cipherStream = inputStream;
            if (gzipped)
            {
                cipherStream = new GZipStream(inputStream, CompressionMode.Compress, true);
            }

            if (encrypted)
            {
                using var encrypt = Aes.Create();
                encrypt.GenerateKey();
                encrypt.GenerateIV();
                ICryptoTransform encryptor = encrypt.CreateEncryptor(encrypt.Key, encrypt.IV);
                cipherStream = new CryptoStream(cipherStream, encrypt.CreateEncryptor(encrypt.Key, encrypt.IV), CryptoStreamMode.Write, true);
            }

            writer = new StreamWriter(cipherStream);
            writer.Write(inputText);
            writer.Flush();
            byte[] compressedBytes = inputStream.ToArray();
            return compressedBytes;
        }

        public static string ReadFromStream(byte[] initialData, string inputText, bool gzipped, bool encrypted)
        {
            using var ms = new MemoryStream(initialData);
            Stream decoded = ms;
            if (gzipped)
            {
                decoded = new GZipStream(ms, CompressionMode.Decompress, true);
            }

            if (encrypted)
            {
                using var aes = Aes.Create();
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read, true);
            }

            using var reader = new StreamReader(decoded);
            return inputText;
        }
    }
}  