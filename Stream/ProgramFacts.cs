using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace StreamProj
{
    public class ProgramFacts
    {
        [Fact]
        public void ReturnInputTextIfInputTextIsNotGzipOrEncrypt()
        {
            string inputText = "jhjhj";
            using MemoryStream stream = new();
            var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            IStreamFactory streamFactory = new StreamFactory(aes);
            var dataStream = streamFactory.CreateWriteStream(stream);
            Program.WriteToStream(dataStream, inputText);
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamFactory(aes).CreateReadStream(stream);
            var result = Program.ReadFromStream(decoded);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsNotEncrypt()
        {
            string inputText = "jhjhj";
            using MemoryStream stream = new();
            var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            IStreamFactory streamFactory = new StreamFactory(aes);
            var dataStream = streamFactory.CreateWriteStream(stream, true);
            Program.WriteToStream(dataStream, inputText, true);
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamFactory(aes).CreateReadStream(stream, true);
            var result = Program.ReadFromStream(decoded, true);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsNotGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            using MemoryStream stream = new();
            var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            IStreamFactory streamFactory = new StreamFactory(aes);
            var dataStream = streamFactory.CreateWriteStream(stream, false, true);
            Program.WriteToStream(dataStream, inputText, false, true);
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamFactory(aes).CreateReadStream(stream, false, true);
            var result = Program.ReadFromStream(decoded, false, true);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            using MemoryStream stream = new();
            var aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
            IStreamFactory streamFactory = new StreamFactory(aes);
            var dataStream = streamFactory.CreateWriteStream(stream, true, true);
            Program.WriteToStream(dataStream, inputText, true, true);   
            stream.Seek(0, SeekOrigin.Begin);
            var decoded = new StreamFactory(aes).CreateReadStream(stream, true, true);
            var result = Program.ReadFromStream(decoded, true, true);
            Assert.Equal(inputText, result);
        }
    }
}
