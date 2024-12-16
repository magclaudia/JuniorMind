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

            if (allChanges.Count > 0)
            {
                if (startIndex < 0 || endIndex > allChanges.Count)
                {
                    endIndex = allChanges.Count - 1;
                }

                int numberOdFiles = 0;

                numberOdFiles = allChanges.Count <= dimensions.unstagedEnd - dimensions.unstagedStart + 1 ? allChanges.Count : endIndex - startIndex + 1;
                allChanges = allChanges.GetRange(startIndex, numberOdFiles);
            }
            else
            {
                allChanges.Clear();
                Console.SetCursorPosition(1, dimensions.unstagedStart);
                string text = Tabs.SetStatusTextLength(" No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                Console.WriteLine(text);
            }

            return allChanges;
        }
    }
}
