namespace GitClient
{
    public class HeaderPanel
    {
        private static DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();


        public static void Header(GetVariablesForCommits indexes)
        {
            if (indexes.right == true)
            {
                Console.SetCursorPosition(1, dimensions.tabHeight + 1);
                Console.Write("Info ");
                Console.SetCursorPosition(1, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2));
                Console.Write("Message ");
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, dimensions.tabHeight + 1);
                Console.Write("Info ");
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2));
                Console.Write("Message ");
            }
        }
    }
}
