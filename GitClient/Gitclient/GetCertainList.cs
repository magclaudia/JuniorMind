namespace GitClient
{
    public class GetCertainList
    {
        public List<string> listOfFiles;
        public List<string> filesNames;
        public List<List<int>> startingIndexes;
        public List<string> addLinesOfCode;
        public List<int> listStartAt;
        public List<int> start;
        public List<string> addList;
        public List<List<string>> listOfAllDiffs;


        public GetCertainList()
        {
            listOfFiles = new List<string>();
            filesNames = new List<string>();
            startingIndexes = new List<List<int>>();
            addLinesOfCode = new List<string>();
            listStartAt = new List<int>();
            start = new List<int>();
            addList = new List<string>();
            listOfAllDiffs = new List<List<string>>();
        }

        public void ClearAllLists()
        {
            listOfFiles.Clear();
            filesNames.Clear();
            startingIndexes.Clear();
            addLinesOfCode.Clear();
            listStartAt.Clear();
            start.Clear();
            addList.Clear();
            listOfAllDiffs.Clear();
        }
    }
}
