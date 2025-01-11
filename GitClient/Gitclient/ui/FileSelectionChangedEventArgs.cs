using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class FileSelectionChangedEventArgs
    {
        public int FileIndex { get; }
        public bool IsStaged { get; }

        public FileSelectionChangedEventArgs(int fileIndex, bool isStaged)
        {
            FileIndex = fileIndex;
            IsStaged = isStaged;
        }
    }
}
