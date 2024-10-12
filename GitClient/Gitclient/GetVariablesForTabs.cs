using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetVariablesForTabs
    {
        public  bool unstageChanges;
        public bool stageChanges;
        public bool initialState;

        public GetVariablesForTabs()
        {
            unstageChanges = false;
            stageChanges = false;
            initialState = false;
        }
    }
}
