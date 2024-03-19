using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestResultsLinq
{
    public class TestResultsFacts
    {
        [Fact]
        public void FamilyResult()
        {
            var list = new List<TestResults>
            {
                new TestResults { Id = "202", FamilyId = "500", Score = 30 },
                new TestResults { Id = "102", FamilyId = "200", Score = 10 },
                new TestResults { Id = "202", FamilyId = "500", Score = 30 },
                new TestResults { Id = "32", FamilyId = "62", Score = 101 },
                new TestResults { Id = "230", FamilyId = "200", Score = 455 },
                new TestResults { Id = "10", FamilyId = "500", Score = 101 }
            };

            var result = TestResults.HighestScore(list);
            var expected = new List<TestResults>
            {
                new TestResults { Id = "10", FamilyId = "500", Score = 101 },
                new TestResults { Id = "230", FamilyId = "200", Score = 455 },
                new TestResults { Id = "32", FamilyId = "62", Score = 101 }
            };

            Assert.Equal(expected, result);
        }
    }
}
