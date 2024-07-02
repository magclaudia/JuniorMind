namespace GitClient
{
    public class Indexes
    {
        public int numberOfCommits;
        public int rightCursor;
        public int cursorPosition;
        public int cursorPositionBiggerThenHeight;
        public int currentCommitIndex;
        public int heightPosition;
        public bool panelAlreadyDisplayed;
        public bool displayPanel;
        public bool up;
        public bool down;
        public Indexes()
        {
            numberOfCommits = 0;
            rightCursor = 1;
            cursorPosition = 0;
            cursorPositionBiggerThenHeight = 0;
            currentCommitIndex = 0;
            heightPosition = 1;
            panelAlreadyDisplayed = false;
            displayPanel = false;
            up = false;
            down = false;
        }
    }
}
