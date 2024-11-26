using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    internal class PanelFactory
    {
        private static UnstagedChangesService unstagedChangesService = new UnstagedChangesService();
        public static UnstangedChangesPanel CreateUnstagedChangesPanel()
        {
            return new UnstangedChangesPanel(unstagedChangesService);
        }
    }
}
