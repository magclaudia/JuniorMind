namespace GitClient
{
    public class GitClient
    {
        static void Main()
        {
            Console.WriteLine("Hello, this is a Git client custom application.\n");
            Console.WriteLine("Choose between the following actions: \n '1' - for displaying total number of commits from your  repository; \n 'e' - if you want to EXIT; \n after that press 'ENTER' to continue.\n");
            Console.WriteLine("Selected command:");
            DisplayFeatures.DisplayTotalNumberOfCommits();
        }
    }
}
