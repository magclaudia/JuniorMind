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
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(stream);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsNotEncrypt()
        {
            string inputText = "jhjhj";
            using MemoryStream stream = new();
            Program.WriteToStream(stream, inputText, true);
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(stream, true);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsNotGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            using MemoryStream stream = new();
            Program.WriteToStream(stream, inputText, false, true);
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(stream, false, true);
            Assert.Equal(inputText, result);
        }

        [Fact]
        public void ReturnInputTextIfInputTextIsGzipAndIsEncrypt()
        {
            string inputText = "jhjhj";
            using MemoryStream stream = new();
            Program.WriteToStream(stream, inputText, true, true);   
            stream.Seek(0, SeekOrigin.Begin);
            var result = Program.ReadFromStream(stream,  true, true);
            Assert.Equal(inputText, result);
        }
    }
}
