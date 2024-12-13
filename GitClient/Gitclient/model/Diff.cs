using GitClient;
using GitClient.ui;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.model
{
    public class Diff
    {
        public List<string> diffs;
        private int currentIndex;
        public Diff(List<string> diffs)
        {
            this.diffs = diffs;
            this.currentIndex = UnstagedChangesPanel.GetCurrentIndex();
        }

        public List<string> Display()
        {
            return diffs;
        }
    }
}
