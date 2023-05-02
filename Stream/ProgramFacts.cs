using Xunit;

namespace Stream
{
    public class ProgramFacts
    {
        [Fact]
        public void ReturnInputTextIfInputTextIsNotGzipOrEncrypt()
        {
            string inputText = "jhjhj";
            bool gzipped = false;
            bool encrypted = false;
            using MemoryStream stream = new();
            var byteInput = Program.WriteToStream(stream, inputText, gzipped, encrypted);
            string cipherText = Convert.ToBase64String(byteInput);
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(byteInput, inputText, gzipped, encrypted);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsNotEncrypt()
        {
            string inputText = "jhjhj";
            bool gzipped = true;
            bool encrypted = false;
            using MemoryStream stream = new();
            var byteInput = Program.WriteToStream(stream, inputText, gzipped, encrypted);
            string cipherText = Convert.ToBase64String(byteInput);
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(byteInput, inputText, gzipped, encrypted);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsNotGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            bool gzipped = false;
            bool encrypted = true;
            using MemoryStream stream = new();
            var byteInput = Program.WriteToStream(stream, inputText, gzipped, encrypted);
            string cipherText = Convert.ToBase64String(byteInput);
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(byteInput, inputText, gzipped, encrypted);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            bool gzipped = true;
            bool encrypted = true;
            using MemoryStream stream = new();
            var byteInput = Program.WriteToStream(stream, inputText, gzipped, encrypted);
            string cipherText = Convert.ToBase64String(byteInput);
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(byteInput, inputText, gzipped, encrypted);
            Assert.Equal(inputText, result);
        }
    }
}
