using GitClient.ui;

namespace GitClient
{
    public class GitClient
    {
        static void Main()
        {
            try
            {
                Ui ui = new Ui();
                Console.Clear();
                ui.Show(UiLayoutTypes.GitStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}. ");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
