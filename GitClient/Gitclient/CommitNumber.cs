using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class CommitNumber
    {
        public static void ReturnCommitNumber(ListOfCommits.CommitElements listOfCommits, ListOfCommits.Indexes indexes)
        {
            int commitNumber = indexes.upOrDownOneStep;
            var text = $"Commit {commitNumber + 1}/{listOfCommits.Id.Count} ";
            Console.SetCursorPosition(1, 0);
            Console.Write(text);
        }
    }
}
