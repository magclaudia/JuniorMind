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
        private CommitsPanel commitsPanel;
        private Ui ui;
        private DiffPanel diffPanel;
        private TabsPanel tabsPanel;
        private string lastFileName = string.Empty;
        private string fileName = string.Empty;
        private int index;
        private string filePath = string.Empty;
        private int hunkIndex;
        private int startIndex;
        private int endIndex;
        private int y;
        private int commitNumber;
        private List<string> hunk = new List<string>();
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

        public void RegisterTabPanel(TabsPanel panel)
        {
            tabsPanel = panel;
        }

        public void RegisterCommitsPanel(CommitsPanel panel)
        {
            commitsPanel = panel;
        }

        public void RegisterUi(Ui panel)
        {
            ui = panel;
        }
        
        public void SetStartIndex(int index)
        {
            startIndex = index;
        }

        public int GetStartIndex()
        {
            return startIndex;
        }

        public void SetEndIndex(int index)
        {
            endIndex = index;
        }

        public int GetEndIndex()
        {
            return endIndex;
        }

        public void SetY(int position)
        {
            y = position;
        }

        public int GetY()
        {
            return y;
        }

        public void SetCommitNumber(int number)
        {
            commitNumber = number;
        }

        public int GetCommitNumber()
        {
            return commitNumber;
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

        public void SetFilePath(string filePathGiven)
        {
            filePath = filePathGiven;
        }

        public string GetFilePath()
        {
            return filePath;
        }

        public int GetCurrentIndex()
        {
            return index;
        }

        public void SetHunkIndex(int hunkIndexGiven)
        {
            hunkIndex = hunkIndexGiven;
        }

        public int GetHunkIndex()
        {
            return hunkIndex;
        }

        public void SetHunkToBeTransfer(List<string> listOfHunk)
        {
            hunk = listOfHunk;
        }

        public List<string> GetHunkToBeTransfer()
        {
            return hunk;
        }

        public void NavigateToCommitsPanel()
        {
            if (commitsPanel != null)
            {
                commitsPanel.Show();
            }
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

        public void NavigateToTabPanel()
        {
            if (tabsPanel != null)
            {
                tabsPanel.Show();
            }
        }

        public void NavigateToStatusInitialState()
        {
            tabsPanel.Show();
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

        public void DisplayLog()
        {
            Console.Clear();
            ui.Show(UiLayoutTypes.LogCommitList);
        }
    }
}
