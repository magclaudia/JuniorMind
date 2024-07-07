namespace GitClient
{
    public class Indexes
    {
        public int startIndex;
        public int rightCursor;
        public int cursorPosition;
        public int currentCommitIndex;
        public int heightPosition;
        public bool panelAlreadyDisplayed;
        public bool displayPanel;
        public bool up;
        public bool down;
        public Indexes()
        {
            startIndex = 0;
            rightCursor = 0;
            cursorPosition = 1;
            currentCommitIndex = 0;
            heightPosition = 1;
            panelAlreadyDisplayed = false;
            displayPanel = false;
            up = false;
            down = false;
        }
    }
}
