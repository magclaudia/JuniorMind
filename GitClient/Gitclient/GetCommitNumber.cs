namespace GitClient
{
    public class GetCommitNumber
    {
        public static void ReturnCommitNumber(CommitElements listOfCommits, GetVariablesForCommits indexes)
        {
            var text = string.Empty;
            int commitNumber = indexes.currentCommitIndex;
            text = $"Commit {commitNumber + 1}/{listOfCommits.Id.Count} ";
            Console.SetCursorPosition(1, 3);
            Console.Write(text);
        }
    }
}
