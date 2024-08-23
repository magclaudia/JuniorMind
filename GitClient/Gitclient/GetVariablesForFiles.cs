using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetVariablesForFiles
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
        public int row;
        public int fileRow;
        public int numberOfNavigations;
        public int x;
        public int lastLine;

        public GetVariablesForFiles()
        {
            nextFile = false;
            end = true;
            fileIndex = 0;
            numberOfFiles = 0;
            up = false;
            down = false;
            indexForFiles = 0;
            countFilesContain = 0;
            currentLine = 1;
            row = 0;
            fileRow = Console.WindowHeight / 2 + 4;
            numberOfNavigations = 0;
            x = 0;
            lastLine = 0;
        }
    }
}
