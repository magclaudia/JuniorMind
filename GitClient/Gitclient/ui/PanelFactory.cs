using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gitclient.repository;

namespace GitClient.ui
{
    public class PanelFactory
    {
        private static LibGit2Repository libGit2Repository = new LibGit2Repository();
        private static UnstagedChangesService unstagedChangesService = new UnstagedChangesService(libGit2Repository);
        
        public static UnstangedChangesPanel CreateUnstagedChangesPanel()
        {
            return new UnstangedChangesPanel(unstagedChangesService);
        }
    }
}
