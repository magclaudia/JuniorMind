using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class FileSelectionChangedEventArgs
    {
        public string FileName { get; }
        public bool IsStaged { get; }

        public FileSelectionChangedEventArgs(string fileName, bool isStaged)
        {
            FileName = fileName;
            IsStaged = isStaged;
        }
    }
}
