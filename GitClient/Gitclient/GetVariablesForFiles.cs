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
        public int indexForLog;
        public int lastLine;
        public int index;
        public int a;
        public int totalLines;
        public int height;
        public int width;
        public bool filesReachPanelLimit;
        public int indexDiff;
        public bool diffMoves;
        public string projName;
        public bool unstageChanges;
        public bool stageChanges;
        public bool statusDiffOpen;
        public bool statusDiffStartNavigate;
        public int currentLineForStatusDiff;
        public int indexForUnstaged;
        public int indexForStaged;
        public int startAt;
        public int stop;
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
            row = 3;
            fileRow = Console.WindowHeight / 2 + 4;
            numberOfNavigations = 0;
            indexForLog = 0;
            lastLine = 0;
            index = 0;
            a = 0;
            totalLines = 0;
            height = Console.WindowHeight - 2;
            width = Console.WindowWidth - 2;
            filesReachPanelLimit = false;
            indexDiff = -1;
            diffMoves = false;
            projName = "";
            unstageChanges = false;
            stageChanges = false;
            statusDiffOpen = false;
            statusDiffStartNavigate = true;
            currentLineForStatusDiff = 1;
            indexForUnstaged = 0;
            indexForStaged = 0;
            startAt = 0;
            stop = 0;
        }
    }
}
