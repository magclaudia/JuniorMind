using Xunit;

namespace DataCollection
{
    public class SortedIntArrayFacts
    {
        [Fact]
        public void SortASmallListOfNumbersWhichBeginWithBiggestDigit()
        {
            var elementsForNewArray = new SortedIntArray();
            elementsForNewArray.Add(4);
            elementsForNewArray.Add(1);
            elementsForNewArray.Add(3);
            elementsForNewArray.Add(2);
            string sortedArray = elementsForNewArray[0] + ", " + elementsForNewArray[1] + ", " + elementsForNewArray[2] + ", " + elementsForNewArray[3];
            Assert.Equal("1, 2, 3, 4", sortedArray);
        }

        [Fact]
        public void SortASmallListOfNumbersWhichBeginWithLowest()
        {
            var elementsForNewArray = new SortedIntArray();
            elementsForNewArray.Add(1);
            elementsForNewArray.Add(3);
            elementsForNewArray.Add(2);
            elementsForNewArray.Add(4);
            string sortedArray = elementsForNewArray[0] + ", " + elementsForNewArray[1] + ", " + elementsForNewArray[2] + ", " + elementsForNewArray[3];
            Assert.Equal("1, 2, 3, 4", sortedArray);
        }

        [Fact]
        public void SortASmallListOfNumbersWithDuplicatesDigits()
        {
            var elementsForNewArray = new SortedIntArray();
            elementsForNewArray.Add(1);
            elementsForNewArray.Add(3);
            elementsForNewArray.Add(3);
            elementsForNewArray.Add(4);
            string sortedArray = elementsForNewArray[0] + ", " + elementsForNewArray[1] + ", " + elementsForNewArray[2] + ", " + elementsForNewArray[3];
            Assert.Equal("1, 3, 3, 4", sortedArray);
        }
    }
}
