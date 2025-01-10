using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.repository;
using GitClient.service;

namespace GitClient.ui
{
    public class PanelFactory
    {
        private static LibGit2Repository libGit2Repository = new LibGit2Repository();
        private static StatusService statusService = new StatusService(libGit2Repository);
        private static UnstageLibGit2DiffRepository unstagelibGit2DiffRepository = new UnstageLibGit2DiffRepository(libGit2Repository);
        private static StageLibGit2DiffRepository stageLibGit2DiffRepository = new StageLibGit2DiffRepository(libGit2Repository);
        private static StatusDiffService statusDiffService = new StatusDiffService(unstagelibGit2DiffRepository, stageLibGit2DiffRepository, libGit2Repository);


        public static UnstagedChangesPanel CreateUnstagedChangesPanel()
        {
            return new UnstagedChangesPanel(statusService, statusDiffService);
        }

        public static StagedChangesPanel CreateStagedChangesPanel()
        {
            return new StagedChangesPanel(statusDiffService, statusService);
        }

        public static DiffPanel StatusDiffPanel()
        {
            return new DiffPanel(statusDiffService, statusService);
        }
    }
}
