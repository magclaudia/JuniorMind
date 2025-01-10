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
        private readonly LibGit2Repository libGit2Repository;
        private DrawTabs.Dimensions dimensions;

        public StatusService(LibGit2Repository libGit2Repository)
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
            List<ChangeAttribute> allChanges = new List<ChangeAttribute>();
            int numberOdFiles = 0;
            int y = 0;
            string textToBeDisplay = "";

            if (typeOfChange == "unstage")
            {
                allChanges = libGit2Repository.GetAllUnstagedChanges();
                numberOdFiles = allChanges.Count <= dimensions.unstagedEnd - dimensions.unstagedStart + 1 ? allChanges.Count : endIndex - startIndex + 1;
                y = dimensions.unstagedStart;
                textToBeDisplay = " No changes found in the unstaging area.";
            }
            else
            {
                allChanges = libGit2Repository.GetAllStageChanges();
                numberOdFiles = allChanges.Count <= dimensions.stagedEnd - dimensions.stagedStart ? allChanges.Count : endIndex - startIndex;
                y = dimensions.stagedStart;
                textToBeDisplay = " No changes found in the staging area.";
            }

            if (allChanges.Count > 0)
            {
                if (startIndex < 0 || endIndex > allChanges.Count)
                {
                    endIndex = allChanges.Count - 1;
                }

                allChanges = allChanges.GetRange(startIndex, numberOdFiles);
            }
            else
            {
                allChanges.Clear();
                Console.SetCursorPosition(1, dimensions.unstagedStart);
                string text = Tabs.SetStatusTextLength(textToBeDisplay, Console.WindowWidth / 2 - 3);
                Console.WriteLine(text);
            }

            return allChanges;
        }

        public List<ChangeAttribute> GetAllStageChanges()
        {
            return libGit2Repository.GetAllStageChanges();
        }

        public void StageFile(ChangeAttribute unstagedFile)
        {
            libGit2Repository.StageUnstagedChange(unstagedFile);
        }
    }
}
