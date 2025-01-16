using GitClient.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.service
{
    public class PanelCommunicationService
    {
        private UnstagedChangesPanel unstagedChangesPanel;
        private StagedChangesPanel stagedChangesPanel;
        private string lastFileName;
        private string fileName;
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public void RegisterUnstagedChangesPanel(UnstagedChangesPanel panel) 
        {
            unstagedChangesPanel = panel;
        }

        public void RegisterStagedChangesPanel(StagedChangesPanel panel)
        {
            stagedChangesPanel = panel;
        }

        //public void SetLastIndexForDiff(int index)
        //{
        //    lastIndexForDiff = index;
        //}

        //public int GetLastIndexForDiff()
        //{
        //    return lastIndexForDiff;
        //}

        public void SetLastUnstageFileName(string currentFileName)
        {
            lastFileName = currentFileName;
        }

        public string GetLastUnstageFileName()
        {
            return lastFileName;
        }

        public void SetCurrentFileName(string currentFileName)
        {
            fileName = currentFileName;
        }

        public string GetCurrentFileName()
        {
            return fileName;
        }

        public void NavigateToUnstagedPanel()
        {
            if (unstagedChangesPanel != null) 
            {
                unstagedChangesPanel.Show();
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.unstagedStart + 2);
                string text = TextSettings.GetTextLength("No changes found in the unstage area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }
        }

        public void NavigateToStagedPanel()
        {
            if (stagedChangesPanel != null)
            {
                stagedChangesPanel.Show();
            }
            else
            {
                Console.SetCursorPosition(2, dimensions.unstagedEnd + 4);
                string text = TextSettings.GetTextLength("No changes found in the stage area.", dimensions.changesPanelWidth - 4);
                Console.Write(text);
            }
        }
    }
}
