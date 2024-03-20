using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestResultsLinq
{
    public class TestResults
    {
        public string Id { get; set; }
        public string FamilyId { get; set; }
        public int Score { get; set; }

        public static IEnumerable<TestResults> HighestScore(IEnumerable<TestResults> list)
        {
            return list.GroupBy(key => key.FamilyId).Select(group => group.MaxBy(x => x.Score));
        }
    }
}
