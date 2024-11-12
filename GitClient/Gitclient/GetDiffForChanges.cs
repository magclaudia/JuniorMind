using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetDiffForChanges
    {
        public static void DiffUnstagedChanges(IntPtr diff, GetCertainList list, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits)
        {

            int result = DiffCallbackForeachFuntion.ReturnForeachCallback(diff, list, variablesForFiles, variablesForCommits);
        }
    }
}
