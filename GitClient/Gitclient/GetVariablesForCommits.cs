namespace GitClient
{
    public class GetVariablesForCommits
    {
        public int startIndex;
        public int rigthCursor;
        public int cursorPosition;
        public int currentCommitIndex;
        public int heightPosition;
        public bool panelAlreadyDisplayed;
        public bool displayPanel;
        public bool up;
        public bool down;
        public bool rigth;
        public int indexForFiles;
        public bool enter;
        public bool nextFile;
        public bool clear;

        public GetVariablesForCommits()
        {
            startIndex = 0;
            rigthCursor = 0;
            cursorPosition = 1;
            currentCommitIndex = 0;
            heightPosition = 1;
            panelAlreadyDisplayed = false;
            displayPanel = false;
            up = false;
            down = false;
            rigth = false;
            indexForFiles = 0;
            enter = false;
            nextFile = false;
            clear = true;
        }
    }
}
