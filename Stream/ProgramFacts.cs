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
            Program.WriteToStream(stream, inputText);
            var result = Program.ReadFromStream(stream);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsNotEncrypt()
        {
            string inputText = "jhjhj";
            bool gzipped;
            using MemoryStream stream = new();
            Program.WriteToStream(stream, inputText, gzipped = true);
            var result = Program.ReadFromStream(stream, gzipped = true);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsNotGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            bool gzipped;
            bool encrypted;
            using MemoryStream stream = new();
            Program.WriteToStream(stream, inputText, gzipped = false, encrypted = true);
            var result = Program.ReadFromStream(stream, gzipped = false, encrypted = true);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            bool gzipped;
            bool encrypted;
            using MemoryStream stream = new();
            Program.WriteToStream(stream, inputText, gzipped = true, encrypted = true);   
            var result = Program.ReadFromStream(stream,  gzipped = true, encrypted = true);
            Assert.Equal(inputText, result);
        }
    }
}
