using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class Ui
    {
        public void Show()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            TabsPanel tabs = new TabsPanel();
            tabs.Show();

            StagedChangesPanel stagedChangesPanel = PanelFactory.CreateStagedChangesPanel();
            DiffPanel diffPanel = PanelFactory.StatusDiffPanel();
            UnstagedChangesPanel unstangedChangesPanel = PanelFactory.CreateUnstagedChangesPanel();

            if (ButtomPress.Type.workingInStagePanel == true)
            {
                int index = stagedChangesPanel.SaveLastIndexForDiff();
                diffPanel.Show(index);
                unstangedChangesPanel.Show();
                stagedChangesPanel.SaveLastYValue();
                stagedChangesPanel.SaveFileNumberLastValue();
                stagedChangesPanel.Show();
            }
            else
            {
                int index = unstangedChangesPanel.SaveLastIndexForDiff();
                diffPanel.Show(index);
                stagedChangesPanel.Show();
                unstangedChangesPanel.SaveLastYValue();
                unstangedChangesPanel.Show();
            }
        }
    }
}
