namespace GitClient
{
    public class BlueBackgroundForCommits
    {
        public static void DisplayBlueBox(List<string> addList, GetVariablesForCommits variablesForCommits, CommitElements listOfCommits)
        {
            if (variablesForCommits.heightPosition > Console.WindowHeight - 2)
            {
                variablesForCommits.heightPosition = Console.WindowHeight - 2;
            }

            Console.SetCursorPosition(1, variablesForCommits.heightPosition);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            if (addList.Count > 2)
            {
                Console.Write(addList[variablesForCommits.heightPosition - 1]);
            }
            else
            {
                if (variablesForCommits.up == true)
                {
                    Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                    Console.Write(addList[0]);
                }
                else if (variablesForCommits.up == true && variablesForCommits.currentCommitIndex > 1)
                {
                    Console.SetCursorPosition(1, 1);
                    Console.Write(addList[0]);
                }
                else
                {
                    Console.Write(addList[1]);
                }
            }

            Console.ResetColor();
        }
    }
}
