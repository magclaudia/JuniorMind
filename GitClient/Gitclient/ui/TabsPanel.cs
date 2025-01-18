using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.repository;
using GitClient.service;

namespace GitClient.ui
{
    public class TabsPanel
    {
        public GetProjectPath projectPath = new GetProjectPath();
        private PanelCommunicationService communicationService = new PanelCommunicationService();

        public void Show()
        {
            DrawTabsPanel();
            GetTabsNames();
            GetProjectPath();
        }

        public void SetCommunicationService(PanelCommunicationService service)
        {
            communicationService = service;
        }

        private void DrawTabsPanel()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

            for (int i = 0; i < dimensions.width; i++)
            {
                Console.SetCursorPosition(i, dimensions.tabHeight);
                Console.Write("─");
            }

            Console.SetCursorPosition(dimensions.tabWidth, dimensions.tabHeight - 1);
            Console.Write("│");
        }

        private void GetTabsNames()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            
            Console.SetCursorPosition(1, 0);
            string statusTab = "Status [1]";
            string outputText = TextSettings.GetTextLength(statusTab, dimensions.tabWidth);
            Console.Write(outputText);

            Console.SetCursorPosition(dimensions.tabWidth + 1, 0);
            string logTab = "    Log [2]";
            outputText = TextSettings.GetTextLength(logTab, dimensions.tabWidth);
            Console.Write(outputText);
        }

        private void GetProjectPath()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            string directoryPath = projectPath.ProjectPath(Environment.CurrentDirectory);

            if (directoryPath == null)
            {
                Console.WriteLine("\nCould not find a Git repository in any directory.");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            string text = TextSettings.GetTextLength(directoryPath, Console.WindowWidth - (dimensions.tabWidth * 2) - 2);
            int startPosition = Console.WindowWidth - 2 - text.Length;
            Console.SetCursorPosition(startPosition, 0);
            Console.Write(text);
            Console.ResetColor();
        }
    }
}
