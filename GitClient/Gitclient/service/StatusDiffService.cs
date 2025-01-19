using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using GitClient.repository;
using GitClient.ui;

namespace GitClient.service
{
    public class StatusDiffService
    {
        private readonly LibGit2RepositoryDiff libGit2RepositoryDiff;
        private readonly LibGit2RepositoryChanges libGit2Repository;
        private PanelCommunicationService communicationService;


        public StatusDiffService(LibGit2RepositoryDiff libGit2RepositoryDiff, LibGit2RepositoryChanges libGit2Repository, PanelCommunicationService communicationService)
        {
            this.libGit2RepositoryDiff = libGit2RepositoryDiff;
            this.libGit2Repository = libGit2Repository;
            this.communicationService = communicationService;
        }

        public List<FileDiff> GetAllStageDiffs()
        {
            return libGit2RepositoryDiff.GetAllStageDiffs();
        }

        public List<FileDiff> GetCurrentDiff(string fileName, string currentPanel)
        {
            List<FileDiff> list = new List<FileDiff>();
            List<FileDiff> diff = new List<FileDiff>();

            if (currentPanel == "unstage")
            {
                diff = libGit2RepositoryDiff.GetAllUnstagedDiff(libGit2Repository.GetDiff());
            }
            else 
            {
                diff = GetAllStageDiffs();
            }

            foreach (var entry in diff)
            {
                if (entry.fileName == fileName)
                {
                    list.Add(entry);
                    communicationService.SetCurrentFileName(entry.fileName);
                    break;
                }
            }

            return list;
        }

        public List<FileDiff> GetAllUnstageDiffs()
        {
            return libGit2RepositoryDiff.GetAllUnstagedDiff(libGit2Repository.GetDiff());
        }
    }
}
