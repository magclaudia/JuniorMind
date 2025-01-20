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

        public void GetHunks()
        {
            string filePath = panelCommunicationService.GetFilePath();
            int hunkIndex = panelCommunicationService.GetHunkIndex();
            List<string> hunk = panelCommunicationService.GetHunkToBeTransfer();
            int fileIndex = panelCommunicationService.GetCurrentIndex();
            repositoryHunks.StageOrUnstageHunk(filePath, hunkIndex, hunk, fileIndex);
        }
    }
}
