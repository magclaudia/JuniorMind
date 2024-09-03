namespace GitClient
{
    public class BlueBackgroundForCommits
    {
        public static void DisplayBlueBox(GetVariablesForCommits variablesForCommits, CommitElements listOfCommits, GetCertainList list)
        {
            if (variablesForCommits.heightPosition > Console.WindowHeight - 2)
            {
                variablesForCommits.heightPosition = Console.WindowHeight - 2;
            }

            Console.SetCursorPosition(1, variablesForCommits.heightPosition);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;

            if (variablesForCommits.up == true && variablesForCommits.currentCommitIndex > 1)
            {
                Console.SetCursorPosition(1, variablesForCommits.heightPosition);
                Console.Write(variablesForCommits.textForBlueFond);

            }
            else if (variablesForCommits.down == true && variablesForCommits.heightPosition <= variablesForCommits.height)
            {
                Console.Write(variablesForCommits.textForBlueFond);
            }
            else
            {
                Console.Write(variablesForCommits.textForBlueFond);
            }

            Console.ResetColor();
        }
    }
}
