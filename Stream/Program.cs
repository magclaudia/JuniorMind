using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace StreamProj
{
    public interface IStreamFactory
    {
        Stream CreateWriteStream(Stream stream, bool gzipped = false, bool encrypted = false);
        Stream CreateReadStream(Stream stream, bool gzipped = false, bool encrypted = false);
    }
    public class Program
    {
        public static void WriteToStream(Stream dataStream, string data, bool gzipped = false, bool encrypted = false)
        {
            var writer = new StreamWriter(dataStream);
            writer.Write(data);
            writer.Flush();

            if (dataStream is CryptoStream cryptoStream)
            {
                cryptoStream.FlushFinalBlock();
            }
        }

        public static string ReadFromStream(Stream decoded, bool gzipped = false, bool encrypted = false)
        {
            var reader = new StreamReader(decoded);
            string convertedString = reader.ReadToEnd();
            reader.Close();
            return convertedString;
        }
    }
}