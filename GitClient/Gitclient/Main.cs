using GitClient.ui;

namespace GitClient
{
    public class GitClient
    {
        static void Main()
        {
            Ui ui = new Ui();
            Console.Clear();
            
            ui.Show(UiLayoutTypes.GitStatus);
         //  Features.DisplayFeatures();
        }
    }
}
