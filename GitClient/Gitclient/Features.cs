namespace GitClient
{
    public class Features
    {
        public static void DisplayFeatures()
        {
            AccessRepository();
        }

        public static void AccessRepository()
        {
            var repoPath = FindDirectoryContainingGitFolder(Environment.CurrentDirectory);

            if (repoPath == null)
            {
                Console.WriteLine("\nCould not find a Git repository in any directory.");
                return;
            }

            LibGit2Wrapper.git_libgit2_init();
            
            CommitElements commitElements = new CommitElements();
            GetVariablesForCommits variablesForCommits = new GetVariablesForCommits();
            GetVariablesForFiles variablesForFiles = new GetVariablesForFiles();
            GetCertainList list = new GetCertainList();

            try
            {
                if (LibGit2Wrapper.git_repository_open(out commitElements.repo, repoPath) != 0)
                {
                    throw new Exception("Failed to open the repository.");
                }

                Tabs.PrintTabs(repoPath, commitElements, variablesForCommits, variablesForFiles, list);
            }
            catch (Exception ex)
            {
                Console.Write($"Error: {ex.Message}");
                Console.Write(ex.StackTrace);
            }

            LibGit2Wrapper.git_repository_free(commitElements.repo);
        }

        public static string? FindDirectoryContainingGitFolder(string directory)
        {
            if (directory == null)
            {
                return null;
            }

            if (Directory.Exists(Path.Combine(directory, ".git")))
            {
                return directory;
            }

            return FindDirectoryContainingGitFolder(Directory.GetParent(directory)!.FullName);
        }
    }
}
