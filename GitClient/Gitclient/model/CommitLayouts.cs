using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.ui;

namespace GitClient.model
{
    public struct CommitLayouts
    {
        public static bool IsFullListOFCommits = false;
        public static bool HaveDescription = false;
        public static bool ChooseCommit = false;
        public static bool IsDisplayFullScreenDiff = false; 
    }
}
