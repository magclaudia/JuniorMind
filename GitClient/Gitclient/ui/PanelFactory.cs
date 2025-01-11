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
        private static PanelCommunicationService panelCommunicationService = new PanelCommunicationService();

        public static UnstagedChangesPanel CreateUnstagedChangesPanel()
        {
            var panel = new UnstagedChangesPanel(statusService, statusDiffService);
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterUnstagedChangesPanel(panel);
            return panel;
            //return new UnstagedChangesPanel(statusService, statusDiffService);
        }

        public static StagedChangesPanel CreateStagedChangesPanel()
        {
            var panel = new StagedChangesPanel(statusDiffService, statusService);
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterStagedChangesPanel(panel);
            return panel;
            //return new StagedChangesPanel(statusDiffService, statusService);
        }

        public static DiffPanel StatusDiffPanel()
        {
            return new DiffPanel(statusDiffService, statusService);
        }
    }
}
