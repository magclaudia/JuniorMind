using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class VariablesForFiles
    {
        public bool end;
        public int fileIndex;
        public int numberOfFiles;
        public bool nextFile;
        public bool up;
        public bool down;
        public int indexForFiles;
        public int countFilesContain;
        public int currentLine;
        public int diffForEachFileIndex;
        public int row;
        public int fileRow;
        public int numberOfNavigations;
        public int max;
        public VariablesForFiles()
        {
            nextFile = false;
            end = true;
            fileIndex = 0;
            numberOfFiles = 0;
            up = false;
            down = false;
            indexForFiles = 0;
            countFilesContain = 0;
            currentLine = 0;
            diffForEachFileIndex = 0;
            row = 0;
            fileRow = Console.WindowHeight / 2 + 4;
            numberOfNavigations = 0;
            max = 0;
        }
    }
}
