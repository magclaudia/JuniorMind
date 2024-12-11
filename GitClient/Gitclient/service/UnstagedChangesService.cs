using Gitclient.model;
using Gitclient.repository;
using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class UnstagedChangesService
    {
        private readonly LibGit2Repository libGit2Repository;

        public UnstagedChangesService(LibGit2Repository libGit2Repository)
        {
            this.libGit2Repository = libGit2Repository;
        }

        public List<UnstagedChange> GetAllUnstagedChanges()
        {
            return libGit2Repository.GetAllUnstagedChanges();
        }

        public List<UnstagedChange> GetCurrentUnstagedChanges(int startIndex, int endIndex)
        {
            List<UnstagedChange> allChanges = libGit2Repository.GetAllUnstagedChanges();

            if (startIndex < 0 || endIndex > allChanges.Count)
            {
                endIndex = allChanges.Count - 1;
            }

            int numberOdFiles = endIndex - startIndex + 1 > 0 ? endIndex - startIndex + 1 : allChanges.Count;
            return allChanges.GetRange(startIndex, numberOdFiles);
        }
    }
}
