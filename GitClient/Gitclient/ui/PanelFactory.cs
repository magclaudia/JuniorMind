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
        private static LibGit2RepositoryChanges libGit2RepositoryChanges = new LibGit2RepositoryChanges();
        private static StatusService statusService = new StatusService(libGit2RepositoryChanges);
        private static LibGit2RepositoryDiff libGit2RepositoryDiff = new LibGit2RepositoryDiff(libGit2RepositoryChanges);
       // private static StageLibGit2DiffRepository stageLibGit2DiffRepository = new StageLibGit2DiffRepository(libGit2Repository);
        private static PanelCommunicationService panelCommunicationService = new PanelCommunicationService();
        private static StatusDiffService statusDiffService = new StatusDiffService(libGit2RepositoryDiff, /*stageLibGit2DiffRepository, */libGit2RepositoryChanges, panelCommunicationService);

        public static UnstagedChangesPanel CreateUnstagedChangesPanel()
        {
            UnstagedChangesPanel panel = new UnstagedChangesPanel(statusService, statusDiffService, panelCommunicationService);
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterUnstagedChangesPanel(panel);
            return panel;
        }

        public static StagedChangesPanel CreateStagedChangesPanel()
        {
            StagedChangesPanel panel = new StagedChangesPanel(statusDiffService, statusService, panelCommunicationService);
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterStagedChangesPanel(panel);
            return panel;
        }

        public static DiffPanel StatusDiffPanel()
        {
            DiffPanel panel = new DiffPanel(statusDiffService, statusService, panelCommunicationService);
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterDiffPanel(panel);
            return panel;
        }
    }
}
