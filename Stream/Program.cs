using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace StreamProj
{
    public interface IStreamBuilder
    {
        Stream CreateWriteStream(Stream stream, bool gzipped, bool encrypted);
        Stream CreateReadStream(Stream stream, bool gzipped, bool encrypted);
    }

    public class Program
    {
        private static readonly Aes aes;
        private static readonly IStreamBuilder streamFactory;

        static Program()
        {
            aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            streamFactory = new StreamFactory(aes);
        }

        public static void WriteToStream(Stream stream, string data, bool gzipped = false, bool encrypted = false)
        {
            var dataStream = streamFactory.CreateWriteStream(stream, gzipped, encrypted);
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
            var decoded = streamFactory.CreateReadStream(stream, gzipped, encrypted);
            var reader = new StreamReader(decoded);
            string convertedString = reader.ReadToEnd();
            reader.Close();
            return convertedString;
        }
    }
}