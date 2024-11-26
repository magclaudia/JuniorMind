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
            UnstangedChangesPanel unstangedChangesPanel = PanelFactory.CreateUnstagedChangesPanel();
            unstangedChangesPanel.Show();
        }
    }
}
