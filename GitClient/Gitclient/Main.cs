using GitClient.ui;

namespace GitClient
{
    public class GitClient
    {
        static void Main()
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Ui ui = new Ui();
                PanelFactory.RegisterUi(ui);
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
