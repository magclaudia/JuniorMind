using System.IO;

namespace GitClient
{
    public class Features
    {
        public static void DisplayFeatures()
        {
            AccessRepository();
        }

        private static void AccessRepository()
        {
            var repoPath = FindDirectoryContainingGitFolder(Environment.CurrentDirectory);

            if (repoPath == null)
            {
                Console.WriteLine("\nCould not find a Git repository in any directory.");
                return;
            }

            LibGit2Wrapper.git_libgit2_init();

            IntPtr repo = IntPtr.Zero;
            try
            {
                if (LibGit2Wrapper.git_repository_open(out repo, repoPath) != 0)
                {
                    throw new Exception("Failed to open the repository.");
                }

                GetListOfCommits.GetAllCommits(repo);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Eroor: {ex.Message}");
            }

            LibGit2Wrapper.git_repository_free(repo);
        }

        private static string? FindDirectoryContainingGitFolder(string directory)
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
