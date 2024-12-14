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
        private DrawTabs.Dimensions dimensions;

        public UnstagedChangesService(LibGit2UnstagedChangesRepository libGit2Repository)
        {
            this.repository = libGit2Repository;
            this.dimensions = new DrawTabs.Dimensions();
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

            int numberOdFiles = 0;

            if (allChanges.Count <= dimensions.unstagedEnd - dimensions.unstagedStart + 1)
            {
                numberOdFiles = allChanges.Count;
            }
            else
            {
               numberOdFiles = endIndex - startIndex + 1;
            }

            return allChanges.GetRange(startIndex, numberOdFiles);
        }
    }
}
