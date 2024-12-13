using Gitclient.ui;
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
            UnstagedChangesPanel unstangedChangesPanel = PanelFactory.CreateUnstagedChangesPanel();
            unstangedChangesPanel.Show();
        }
    }
}
