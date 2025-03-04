using GitClient.model;
using GitClient.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.service
{
    public class HunksService
    {
        private LibGit2RepositoryHunks repositoryHunks;
        private PanelCommunicationService panelCommunicationService;

        public HunksService(LibGit2RepositoryHunks repositoryHunks, PanelCommunicationService panelCommunicationService) 
        {
            this.repositoryHunks = repositoryHunks;
            this.panelCommunicationService = panelCommunicationService;
        }

        public void StageHunk()
        {
            int hunkIndex = panelCommunicationService.GetHunkIndex();
            string line = panelCommunicationService.GetLineToBeStaged();
            repositoryHunks.StageHunk(hunkIndex, line);
        }
        
        public void UnstageHunk()
        {
            int hunkIndex = panelCommunicationService.GetHunkIndex();
            string line = panelCommunicationService.GetLineToBeStaged();
            repositoryHunks.UnstageHunk(hunkIndex, line);
        }
    }
}
