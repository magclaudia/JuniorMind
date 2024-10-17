namespace GitClient
{
    public class GetCertainList
    {
        public List<string> listOfFiles;
        public List<string> filesNames;
        public List<List<int>> startingIndexesLog;
        public List<string> addLinesOfCode;
        public List<int> listStartAt;
        public List<int> start;
        public List<string> addList;
        public List<List<string>> listOfAllDiffs;
        public List<string> unstagedChangesFiles;
        public List<string> stagedChangesFiles;
        public List<List<string>> unstagedChangesDiff;
        public List<List<string>> stagedChangesDiff;
        public List<List<int>> startingIndexesUnstaged;
        public List<List<int>> startingIndexesStaged;

        public GetCertainList()
        {
            listOfFiles = new List<string>();
            filesNames = new List<string>();
            startingIndexesLog = new List<List<int>>();
            addLinesOfCode = new List<string>();
            listStartAt = new List<int>();
            start = new List<int>();
            addList = new List<string>();
            listOfAllDiffs = new List<List<string>>();
            unstagedChangesFiles = new List<string>();
            stagedChangesFiles = new List<string>();
            unstagedChangesDiff = new List<List<string>>();
            stagedChangesDiff = new List<List<string>>();
            startingIndexesUnstaged = new List<List<int>>();
            startingIndexesStaged = new List<List<int>>();
        }

        public void ClearAllLists()
        {
            listOfFiles.Clear();
            filesNames.Clear();
            startingIndexesLog.Clear();
            addLinesOfCode.Clear();
            listStartAt.Clear();
            start.Clear();
            addList.Clear();
            listOfAllDiffs.Clear();
            unstagedChangesFiles.Clear();
            stagedChangesFiles.Clear();
            unstagedChangesDiff.Clear();
            stagedChangesDiff.Clear();
            startingIndexesUnstaged.Clear();
            startingIndexesStaged.Clear();
        }
    }
}
