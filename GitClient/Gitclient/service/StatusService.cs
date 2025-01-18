using GitClient.model;
using GitClient.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class StatusService
    {
        private readonly LibGit2RepositoryChanges libGit2Repository;
        private DrawTabs.Dimensions dimensions;

        public StatusService(LibGit2RepositoryChanges libGit2Repository)
        {
            this.libGit2Repository = libGit2Repository;
            dimensions = new DrawTabs.Dimensions();
        }

        public List<ChangeAttribute> GetAllUnstagedChanges()
        {
            return libGit2Repository.GetAllUnstagedChanges();
        }

        public List<ChangeAttribute> GetCurrentChanges(int startIndex, int endIndex, string typeOfChange)
        {
            List<ChangeAttribute> listOfChanges = new List<ChangeAttribute>();
            int count = 0;
            int y = 0;

            if (typeOfChange == "unstage")
            {
                listOfChanges = libGit2Repository.GetAllUnstagedChanges();
                int maxVisibleChanges = dimensions.unstagedEnd - dimensions.unstagedStart + 1;

                count = Math.Min(maxVisibleChanges, listOfChanges.Count - startIndex);

                if (count + startIndex > listOfChanges.Count)
                {
                    count--;
                }

                y = dimensions.unstagedStart;
            }
            else
            {
                listOfChanges = libGit2Repository.GetAllStageChanges();
                int maxVisibleChanges = dimensions.stagedEnd - dimensions.stagedStart;

                count = Math.Min(maxVisibleChanges, listOfChanges.Count - startIndex);

                if (count + startIndex > listOfChanges.Count)
                {
                    count--;
                }

                y = dimensions.stagedStart;
            }

            if (listOfChanges.Count > 0)
            {
                if (startIndex >= 0 && count > 0)
                {
                    listOfChanges = listOfChanges.GetRange(startIndex, count);
                }
                else
                {
                    listOfChanges = new List<ChangeAttribute>(); 
                }
            }
            
            return listOfChanges;
        }

        public List<ChangeAttribute> GetAllStageChanges()
        {
            return libGit2Repository.GetAllStageChanges();
        }

        public void StageFile(ChangeAttribute unstagedFile)
        {
            libGit2Repository.StageFile(unstagedFile);
        }

        public void UnstageFile(ChangeAttribute stageFile)
        {
            libGit2Repository.UnstageFile(stageFile);
        }
    }
}
