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
        public bool left;
        public int index;
        public int a;
        public int totalLines;
        public int height;
        public int width;

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
            left = false;
            index = 0;
            a = 0;
            totalLines = 0;
            height = Console.WindowHeight - 2;
            width = Console.WindowWidth - 2;
        }
    }
}
