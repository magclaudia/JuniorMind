using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gitclient.repository;
using Gitclient.ui;
using Gitclient.service;

namespace GitClient.ui
{
    public class PanelFactory
    {
        private static LibGit2UnstagedChangesRepository libGit2RepositoryForChanges = new LibGit2UnstagedChangesRepository();
        private static UnstagedChangesService unstagedChangesService = new UnstagedChangesService(libGit2RepositoryForChanges);
       
        private static LibGit2UnstagedDiffRepository libGit2RepositoryUnstagedDiff = new LibGit2UnstagedDiffRepository();
        private static UnstagedChangesDiffService unstagedDiffService = new UnstagedChangesDiffService(libGit2RepositoryUnstagedDiff, libGit2RepositoryForChanges);
        
        public static UnstagedChangesPanel CreateUnstagedChangesPanel()
        {
            return new UnstagedChangesPanel(unstagedChangesService, unstagedDiffService);
        }

        public static UnstagedDiffPanel CreateUnstagedChangesDiffPanel()
        {
            return new UnstagedDiffPanel(unstagedDiffService, unstagedChangesService);
        }
    }
}
