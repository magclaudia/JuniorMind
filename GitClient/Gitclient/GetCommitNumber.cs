namespace GitClient
{
    public class GetCommitNumber
    {
        public static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public static void ReturnCommitNumber(CommitElements listOfCommits, GetVariablesForCommits indexes)
        {
            var text = string.Empty;
            int commitNumber = indexes.currentCommitIndex;
            text = $"Commit {commitNumber + 1}/{listOfCommits.Id.Count} ";
            Console.SetCursorPosition(1, dimensions.tabHeight + 1);
            Console.Write(text);
        }
    }
}
