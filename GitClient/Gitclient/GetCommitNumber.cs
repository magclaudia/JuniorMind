using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetCommitNumber
    {
        public static void ReturnCommitNumber(CommitElements listOfCommits, GetVariablesForCommits indexes)
        {
            var text = string.Empty;
            int commitNumber = indexes.currentCommitIndex;
            text = $"Commit {commitNumber + 1}/{listOfCommits.Id.Count} ";
            Console.SetCursorPosition(1, 0);
            Console.Write(text);
        }
    }
}
