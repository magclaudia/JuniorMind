using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient
{
    public class CommitNumber
    {
        public static void ReturnCommitNumber(List<string> listOfCommits, int upOrDownOneStep)
        {
            int commitNumber = upOrDownOneStep + 1;
            var text = $"Commit {commitNumber}/{listOfCommits.Count} ";
            Console.SetCursorPosition(1, 0);
            Console.Write(text);
        }
    }
}
