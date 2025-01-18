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
        private DiffPanel diffPanel;
        private string lastFileName;
        private string fileName;
        private int index;
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public void RegisterUnstagedChangesPanel(UnstagedChangesPanel panel) 
        {
            unstagedChangesPanel = panel;
        }

        public void RegisterStagedChangesPanel(StagedChangesPanel panel)
        {
            stagedChangesPanel = panel;
        }

        public void RegisterDiffPanel(DiffPanel panel)
        {
            diffPanel = panel;  
        }

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

        public void SetCurrentIndex(int currentIndex)
        {
            index = currentIndex;
        }

        public int GetCurrentIndex()
        {
            return index;
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

        public void NavigateToDiffPanel()
        {
            if (diffPanel != null)
            {
                diffPanel.Show();
            }
        }

        public void NavigateToStatusInitialState()
        {
            diffPanel.Show();

            if (ButtomPress.Type.workingInStagePanel == true)
            {
                unstagedChangesPanel.Show();
                stagedChangesPanel.Show();
            }
            else
            {
                stagedChangesPanel.Show();
                unstagedChangesPanel.Show();
            }
        }
    }
}
