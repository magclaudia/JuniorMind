using Xunit;

namespace DataCollection
{
    public class DictionaryFacts
    {
        [Fact]
        public void TestAdd()
        {
            var dictionary = new Dictionary<int, int>(5);
            dictionary.Add(1, 2);
            dictionary.Add(2, 3);
            dictionary.Add(3, 4);
            dictionary.Add(4, 5);
            dictionary.Add(5, 6);
            Assert.Equal(5, dictionary.Count);
        }

        [Fact]
        public void TestClear()
        {
            var dictionary = new Dictionary<int, int>(5);
            dictionary.Add(1, 2);
            dictionary.Add(2, 3);
            dictionary.Add(3, 4);
            dictionary.Add(4, 5);
            dictionary.Add(5, 6);
            Assert.Equal(5, dictionary.Count);
            dictionary.Clear();
            Assert.Equal(0, dictionary.Count);
        }

        [Fact]
        public void TestContains() 
        {
            var dictionary = new Dictionary<int, int>(5);
            var pair = new KeyValuePair<int, int>(1, 2);
            Assert.False(dictionary.Contains(pair));
            dictionary.Add(pair);
            Assert.True(dictionary.Contains(pair));
        }

        [Fact]
        public void TestContainsKey()
        {
            var dictionary = new Dictionary<int, int>(5);
            dictionary.Add(1, 2);
            dictionary.Add(2, 3);
            dictionary.Add(3, 4);
            dictionary.Add(4, 5);
            dictionary.Add(5, 6);
            Assert.Equal(5, dictionary.Count);
            Assert.True(dictionary.ContainsKey(3));
            Assert.False(dictionary.ContainsKey(7));
        }
    }
}
