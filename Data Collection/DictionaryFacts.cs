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
            var dictionary = new Dictionary<object, int>(5);
            var pair = new KeyValuePair<object, int>(1, 2);
            Assert.False(dictionary.Contains(pair));
            dictionary.Add(pair);
            Assert.True(dictionary.Contains(pair));
            var addNewPair = new KeyValuePair<object, int>(null, 2);
            Assert.Throws<ArgumentNullException>(() => dictionary.Contains(addNewPair));
        }

        [Fact]
        public void TestContainsKey()
        {
            var dictionary = new Dictionary<object, int>(5);
            dictionary.Add(1, 2);
            dictionary.Add(2, 3);
            dictionary.Add(3, 4);
            dictionary.Add(4, 5);
            dictionary.Add(5, 6);
            Assert.Equal(5, dictionary.Count);
            Assert.True(dictionary.ContainsKey(3));
            Assert.False(dictionary.ContainsKey(7));
            Assert.Throws<ArgumentNullException>(() => dictionary.ContainsKey(null));
        }

        [Fact]
        public void TestCopyTo()
        {
            var dictionary = new Dictionary<int, int>(2);
            dictionary.Add(1, 2);
            dictionary.Add(2, 3);
            var array = new KeyValuePair<int, int>[2];
            Assert.Equal(2, dictionary.Count);
            dictionary.CopyTo(array, 0);
            KeyValuePair<int, int>[] result = { new KeyValuePair<int, int>(1, 2), new KeyValuePair<int, int>(2, 3) };
            Assert.Equal(result, array);
        }

        [Fact]
        public void TestRemove1()
        {
            var dictionary = new Dictionary<object, int>(5);
            dictionary.Add(1, 2);
            dictionary.Add(2, 3);
            dictionary.Add(10, 4);
            dictionary.Add(7, 5);
            dictionary.Add(12, 6);
            Assert.Equal(5, dictionary.Count);
            dictionary.Remove(13);
            Assert.Equal(5, dictionary.Count);
            dictionary.Remove(12);
            Assert.Equal(4, dictionary.Count);
            dictionary.Add(12, 6);
            Assert.Equal(5, dictionary.Count);
        }

        [Fact]
        public void TestRemove2()
        {
            var dictionary = new Dictionary<object, int>(5);
            dictionary.Add(1, 2);
            dictionary.Add(2, 3);
            dictionary.Add(10, 4);
            dictionary.Add(7, 5);
            dictionary.Add(12, 6);
            Assert.Equal(5, dictionary.Count);
            dictionary.Remove(7);
            Assert.Equal(4, dictionary.Count);
        }
    }
}
