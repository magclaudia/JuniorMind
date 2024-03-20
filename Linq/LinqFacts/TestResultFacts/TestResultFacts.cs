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
            var a = new TestResults { Id = "202", FamilyId = "500", Score = 30 };
            var b = new TestResults { Id = "102", FamilyId = "200", Score = 10 };
            var c = new TestResults { Id = "202", FamilyId = "500", Score = 30 };
            var d = new TestResults { Id = "32", FamilyId = "62", Score = 101 };
            var e = new TestResults { Id = "230", FamilyId = "200", Score = 455 };
            var f = new TestResults { Id = "10", FamilyId = "500", Score = 101 };

            var list = new List<TestResults>
            {
               a,b,c,d,e,f
            };

            var result = TestResults.HighestScore(list);
            var expected = new List<TestResults>
            {
                f,e,d 
            };

            Assert.Equal(expected,result);
        }
    }
}
