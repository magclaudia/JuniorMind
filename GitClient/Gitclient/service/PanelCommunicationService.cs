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
        private LogPanel commitsPanel;
        private Ui ui;
        private DiffPanel diffPanel;
        private TabsPanel tabsPanel;
        private string lastFileName = string.Empty;
        private string fileName = string.Empty;
        private int fileIndex;
        private int diffIndex;
        private string filePath = string.Empty;
        private int hunkIndex;
        private string line = "";
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
        public void RegisterCommitsPanel(LogPanel panel)
        {
            commitsPanel = panel;
        }
        public void RegisterUi(Ui panel)
        {
            ui = panel;
        }
        public void SetLastUnstageFileName(string currentFileName)
        {
            lastFileName = currentFileName;
        }
        public void SetCurrentFileName(string currentFileName)
        {
            fileName = currentFileName;
        }
        public string GetCurrentFileName()
        {
            return fileName;
        }
        public void SetFileIndex(int currentIndex)
        {
            fileIndex = currentIndex;
        }
        public int GetFileIndex()
        {
            return fileIndex;
        }
        public void SetDiffIndex(int diffindex)
        {
            diffIndex = diffindex;
        }
        public int GetDiffIndex()
        {
            return diffIndex;
        }
        public void SetFilePath(string filePathGiven)
        {
            filePath = filePathGiven;
        }
        public string GetFilePath()
        {
            return filePath;
        }
        public void SetHunkIndex(int hunkIndexGiven)
        {
            hunkIndex = hunkIndexGiven;
        }
        public int GetHunkIndex()
        {
            return hunkIndex;
        }
        public void SetLineToBeStaged(string lineForTransfer)
        {
            line = lineForTransfer;
        }
        public string GetLineToBeStaged()
        {
            return line;
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

            if (ReadButtons.WorkingInStagePanel == true)
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
            ui.Show(UiLayoutTypes.GitLog);
        }
    }
}
