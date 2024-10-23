namespace GitClient
{
    public class Info
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public static void GetInfo(GetVariablesForCommits indexes, CommitElements listOfCommits)
        {
            var position = new DrawPanelRigthSide.InfoPanel();
            string[] infos = { "Author", "Date/Time", "Sha" };
            string author = listOfCommits.Author[indexes.currentCommitIndex];
            string dateOrTime = listOfCommits.DateTime[indexes.currentCommitIndex];
            string sha = listOfCommits.Id[indexes.currentCommitIndex];
            string[] arrayOfInfos = { author, dateOrTime, sha };
            string output;

            for (int i = dimensions.tabHeight + 2; i < position.height; i++)
            {
                if (indexes.right == true)
                {
                    Console.SetCursorPosition(1, i);
                }
                else
                {
                    Console.SetCursorPosition(position.edgeOne + 1, i);
                }

                output = $"{infos[i - 3]}: {arrayOfInfos[i - 3]}";
                if (output.Length > position.width)
                {
                    output = output.Substring(0, position.width);
                }
                else
                {
                    output = output.Substring(0, output.Length);
                }

                Console.Write(output);

                if (i == 5)
                {
                    break;
                }
            }
        }
    }
}
