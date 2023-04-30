using NUnit.Framework;
using Xunit;

namespace Stream
{
    public class ProgramFacts
    {
        [Fact]
        public void VerifyIfRetunInputStringIfTheTextIsNOtGzipatOrEncrypt()
        {
            using var stream = new MemoryStream();
            var gzipped = false;
            var encrypted = false;
            new StreamReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(stream);
            Assert.Equals("input text", result);
        }
    }
}
