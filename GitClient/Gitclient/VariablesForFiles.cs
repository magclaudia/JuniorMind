using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class VariablesForFiles
    {
        public bool filesCode;
        public bool end;
        public int fileIndex;
        public int diffIndex;
        public int numberOfFiles;
        public bool nextFile;
        public bool up;
        public bool down;
        public int indexForFiles;
        public int countFilesContain;

        public VariablesForFiles()
        {
            nextFile = false;
            filesCode = false;
            end = false;
            fileIndex = 0;
            diffIndex = 0;
            numberOfFiles = 0;
            up = false;
            down = false;
            indexForFiles = 0;
            countFilesContain = 0;
        }
    }
}
