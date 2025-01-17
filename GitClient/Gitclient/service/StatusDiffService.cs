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
        private readonly UnstageLibGit2DiffRepository unstageDiffRepository;
        private readonly StageLibGit2DiffRepository stageDiffRepository;
        private readonly LibGit2Repository libGit2Repository;
        private PanelCommunicationService communicationService;
        private string fileName;

        public StatusDiffService(UnstageLibGit2DiffRepository unstageDiffRepositoryDiff, StageLibGit2DiffRepository stageDiffRepository, LibGit2Repository libGit2Repository, PanelCommunicationService communicationService)
        {
            this.unstageDiffRepository = unstageDiffRepositoryDiff;
            this.libGit2Repository = libGit2Repository;
            this.stageDiffRepository = stageDiffRepository;
            this.communicationService = communicationService;
            fileName = string.Empty;
        }

        public List<FileDiff> GetAllStageDiffs()
        {
            return stageDiffRepository.GetAllStageDiffs();
        }

        public List<FileDiff> GetCurrentDiff(string fileName, string currentPanel)
        {
            List<FileDiff> list = new List<FileDiff>();
            List<FileDiff> diff = new List<FileDiff>();

            if (currentPanel == "unstage")
            {
                diff = unstageDiffRepository.GetAllUnstagedDiff(libGit2Repository.GetDiff());
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

        //public List<FileDiff> GetCurrentStageDiff(string fileName)
        //{
        //    List<FileDiff> list = new List<FileDiff>();
        //    List<FileDiff> diff = GetAllStageDiffs();

        //    foreach(var entry in diff)
        //    {
        //        if (entry.fileName == fileName)
        //        {
        //            list.Add(entry);
        //            communicationService.SetCurrentFileName(entry.fileName);
        //            break;
        //        }
        //    }

        //    return list;
        //}

        public List<FileDiff> GetAllUnstageDiffs()
        {
            return unstageDiffRepository.GetAllUnstagedDiff(libGit2Repository.GetDiff());
        }

        //public List<FileDiff> GetCurrentUnstageDiff(string fileName)
        //{
        //    List<FileDiff> list = new List<FileDiff>();
        //    List<FileDiff> diff = unstageDiffRepository.GetAllUnstagedDiff(libGit2Repository.GetDiff());
            
        //    foreach (var entry in diff)
        //    {
        //        if (entry.fileName == fileName)
        //        {
        //            list.Add(entry);
        //            SetCurrentFileName(entry.fileName);
        //            break;
        //        }
        //    }

        //    return list;
        //}

        public void SetCurrentFileName(string file) 
        {
            fileName = file;    
        }
    }
}
