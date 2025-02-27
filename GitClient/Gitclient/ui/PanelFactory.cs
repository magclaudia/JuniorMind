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
        private static PanelCommunicationService panelCommunicationService = new PanelCommunicationService();
        private static StatusDiffService statusDiffService = new StatusDiffService(libGit2RepositoryDiff, libGit2RepositoryChanges, panelCommunicationService);
        
        private static LibGit2RepositoryHunks repositoryHunks = new LibGit2RepositoryHunks(/*libGit2RepositoryChanges, libGit2RepositoryDiff, */panelCommunicationService);
        private static HunksService hunksService = new HunksService(repositoryHunks, panelCommunicationService);

        private static CommitsLibGit2Repository commitsLibGit2Repository = new CommitsLibGit2Repository();
        private static CommitsService commitsService = new CommitsService(commitsLibGit2Repository);


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

        public static LogPanel CreateLogPanel()
        {
            LogPanel panel = new LogPanel(commitsService, panelCommunicationService);
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterCommitsPanel(panel);
            return panel;    
        }

        public static CommitsNavigation CreateCommitsNavigation()
        {
            CommitsNavigation navigation = new CommitsNavigation(commitsService, panelCommunicationService);   
            return navigation;
        }

        public static CommitsWithDescription CommitsWithDescription() 
        {
            CommitsWithDescription panel = new CommitsWithDescription(commitsService, panelCommunicationService);
            return panel;
        }

        public static DiffPanel StatusDiffPanel()
        {
            DiffPanel panel = new DiffPanel(statusDiffService, statusService, hunksService, panelCommunicationService);
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterDiffPanel(panel);
            return panel;
        }

        public static TabsPanel Tabs()
        {
            TabsPanel panel = new TabsPanel();
            panel.SetCommunicationService(panelCommunicationService);
            panelCommunicationService.RegisterTabPanel(panel);
            return panel;
        }

        public static void RegisterUi(Ui ui)
        {
            panelCommunicationService.RegisterUi(ui);
        }
    }
}
