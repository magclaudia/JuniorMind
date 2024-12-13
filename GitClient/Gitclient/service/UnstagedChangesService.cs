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
        private readonly LibGit2UnstagedChangesRepository repository;

        public UnstagedChangesService(LibGit2UnstagedChangesRepository libGit2Repository)
        {
            this.repository = libGit2Repository;
        }

        public List<UnstagedChange> GetAllUnstagedChanges()
        {
            return repository.GetAllUnstagedChanges();
        }

        public List<UnstagedChange> GetCurrentUnstagedChanges(int startIndex, int endIndex)
        {
            List<UnstagedChange> allChanges = repository.GetAllUnstagedChanges();

            if (startIndex < 0 || endIndex > allChanges.Count)
            {
                endIndex = allChanges.Count - 1;
            }

            int numberOdFiles = endIndex - startIndex + 1 > 0 ? endIndex - startIndex + 1 : allChanges.Count - 1;
            return allChanges.GetRange(startIndex, numberOdFiles);
        }
    }
}
