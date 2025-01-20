using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.service;

namespace GitClient.ui
{
    public class Ui
    {
        private Dictionary<UiLayoutTypes, List<UiComponent>> layoutTypes = new Dictionary<UiLayoutTypes, List<UiComponent>>();
        private UnstagedChangesPanel unstangedChangesPanel;
        private StagedChangesPanel stagedChangesPanel;
        private DiffPanel diffPanel;
        private TabsPanel tabsPanel;
        private CommitsPanel commitsPanel;

        public Ui() 
        {
            unstangedChangesPanel = PanelFactory.CreateUnstagedChangesPanel();
            stagedChangesPanel = PanelFactory.CreateStagedChangesPanel();
            commitsPanel = PanelFactory.CreateCommitsPanel();
            diffPanel = PanelFactory.StatusDiffPanel();
            diffPanel.SubcribeToPanel(unstangedChangesPanel, stagedChangesPanel);
            tabsPanel = PanelFactory.Tabs();
            layoutTypes.Add(UiLayoutTypes.GitStatus, new List<UiComponent>() { diffPanel, stagedChangesPanel, unstangedChangesPanel });
            layoutTypes.Add(UiLayoutTypes.DiffStatus, new List<UiComponent>() { diffPanel });
            layoutTypes.Add(UiLayoutTypes.LogCommitList, new List<UiComponent> { commitsPanel });
        }

        public void Show(UiLayoutTypes layout)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            TabsPanel tabs = new TabsPanel();
            tabs.Show();

            if (layoutTypes.ContainsKey(layout))
            {
                List<UiComponent> panels = layoutTypes[layout];
                
                foreach(var panel in panels)
                {
                    panel.Show();
                }
            }
            else
            {
                throw new Exception("Key not found.");
            }
        }
    }
}
