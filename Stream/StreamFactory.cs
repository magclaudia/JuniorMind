using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace StreamProj
{
    public class StreamFactory : IStreamFactory
    {
        private readonly Aes aes;

        public StreamFactory(Aes aes)
        {
            this.aes = aes;
        }

        public Stream CreateWriteStream(Stream stream, bool gzipped = false, bool encrypted = false)
        {
            Stream dataStream = stream;
            if (gzipped)
            {
                dataStream = new GZipStream(dataStream, CompressionMode.Compress);
            }

            if (encrypted)
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                dataStream = new CryptoStream(dataStream, encryptor, CryptoStreamMode.Write);
            }

            return dataStream;
        }

        public Stream CreateReadStream(Stream stream, bool gzipped = false, bool encrypted = false)
        {
            Stream dataStream = stream;

            if (gzipped)
            {
                dataStream = new GZipStream(dataStream, CompressionMode.Decompress);
            }

            if (encrypted)
            {
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                dataStream = new CryptoStream(dataStream, decryptor, CryptoStreamMode.Read);
            }

            return dataStream;
        }
    }
}
