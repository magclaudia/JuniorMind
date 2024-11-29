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
        public int diffStartAt;
        public int filesStatusStartAt;
        public int fileLogStartAt;
        public int stop;
        public bool initialState;
        public bool finishUpMoves;
        public string filePath;
        public bool statusFilesSufferModifications;
        public int unstagedIndex;
        public int stagedIndex;
        public int fileRowUnstaged;
        public int fileRowStaged;
        public int enterPress;


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
            row = 2;
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
            projName = string.Empty;
            unstageChanges = false;
            stageChanges = false;
            statusDiffOpen = false;
            statusDiffStartNavigate = true;
            currentLineForStatusDiff = 1;
            indexForUnstaged = 0;
            indexForStaged = 0;
            diffStartAt = 0;
            filesStatusStartAt = 0;
            fileLogStartAt = 0;
            stop = 0;
            initialState = false;
            finishUpMoves = false;
            filePath = string.Empty;
            statusFilesSufferModifications = false;
            unstagedIndex = 0;
            stagedIndex = 0;
            fileRowUnstaged = 4;
            fileRowStaged = 11;
            enterPress = 0;
        }
    }
}
