namespace GitClient
{
    public class GetCertainList
    {
        public List<string> listOfFiles;
        public List<string> filesNames;
        public List<List<int>> startingIndexesLog;
        public List<string> addLinesOfCode;
        public List<int> start;
        public List<string> addList;
        public List<List<string>> listOfAllDiffs;
        public List<string> unstagedChangesFiles;
        public List<string> stagedChangesFiles;
        public List<List<string>> unstagedChangesDiff;
        public List<List<string>> stagedChangesDiff;
        public List<List<int>> unstagedDiffListStartAt;
        public List<List<int>> stagedDiffListStartAt;
        public List<int> unstagedFilesStartAt;
        public List<int> stagedFilesStartAt;
        public List<int> logFilesStartAt;

        public GetCertainList()
        {
            listOfFiles = new List<string>();
            filesNames = new List<string>();
            startingIndexesLog = new List<List<int>>();
            addLinesOfCode = new List<string>();
            start = new List<int>();
            addList = new List<string>();
            listOfAllDiffs = new List<List<string>>();
            unstagedChangesFiles = new List<string>();
            stagedChangesFiles = new List<string>();
            unstagedChangesDiff = new List<List<string>>();
            stagedChangesDiff = new List<List<string>>();
            unstagedDiffListStartAt = new List<List<int>>();
            stagedDiffListStartAt = new List<List<int>>();
            unstagedFilesStartAt = new List<int>();
            stagedFilesStartAt = new List<int>();
            logFilesStartAt = new List<int>();
        }

        public void ClearAllLists()
        {
            listOfFiles.Clear();
            filesNames.Clear();
            startingIndexesLog.Clear();
            addLinesOfCode.Clear();
            start.Clear();
            addList.Clear();
            listOfAllDiffs.Clear();
            unstagedChangesFiles.Clear();
            stagedChangesFiles.Clear();
            unstagedChangesDiff.Clear();
            stagedChangesDiff.Clear();
            unstagedDiffListStartAt.Clear();
            stagedDiffListStartAt.Clear();
            unstagedDiffListStartAt.Clear();
            unstagedFilesStartAt.Clear();
            stagedChangesDiff.Clear();
            logFilesStartAt.Clear();
        }
    }
}
