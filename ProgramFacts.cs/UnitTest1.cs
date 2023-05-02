
namespace ReadAndWriteProject
{
    public class ProgramFacts
    {
        [Fact]
        public void VerifyIfRetunInputStringIfTheTextIsNOtGzipatOrEncrypt()
        {
            string inputText = "jfhfskf";
            bool gzipped = false;
            bool encrypted = false;
            using var stream = new MemoryStream();
            WriteToStream(stream, inputText, gzipped, encrypted);
            var result = ReadFromStream(stream, gzipped, encrypted);
            Assert.Equal(inputText, result);
        }
    }
}