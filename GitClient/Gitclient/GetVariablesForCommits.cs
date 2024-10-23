namespace GitClient
{
    public class GetVariablesForCommits
    {
        public int rigthCursor;
        public int cursorPosition;
        public int currentCommitIndex;
        public int heightPosition;
        public bool panelAlreadyDisplayed;
        public bool displayPanel;
        public bool up;
        public bool down;
        public bool right;
        public int indexForFiles;
        public bool enter;
        public bool nextFile;
        public bool clear;
        public int numberOfEnterPresses;
        public int height;
        public int width;
        public string textForBlueFond;
        public int pressRight;
        public bool stopWorkingOnCommits;
        public bool left;
        public bool esc;
        public int fileIndex;
        public bool logTab;
        public int firstCommitInLine;

        public GetVariablesForCommits()
        {
            rigthCursor = 0;
            cursorPosition = 1;
            currentCommitIndex = 0;
            heightPosition = 3;
            panelAlreadyDisplayed = false;
            displayPanel = false;
            up = false;
            down = false;
            right = false;
            indexForFiles = 0;
            enter = false;
            nextFile = false;
            clear = true;
            numberOfEnterPresses = 0;
            height = Console.WindowHeight - 4;
            width = Console.WindowWidth;
            textForBlueFond = string.Empty;
            pressRight = 0;
            stopWorkingOnCommits = false;
            left = false;
            esc = false;
            fileIndex = 0;
            logTab = false;
            firstCommitInLine = 0;
        }
    }
}
