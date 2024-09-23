using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Testing_git_diff_foreach_API
{
    public class MainClass
    {
        public static void Main()
        {
            IntPtr repo = AccessRepository();
            var element = new Get_CommitElement.Elements();
            IntPtr commitPtr = GetCommit.GetFirstCommit(repo, element);
            IntPtr diff = GetFiles.GetFilesAffectedByCommit(repo, commitPtr, element);
            GetDiff.GetFirstDiff(diff);

            LibGit2Wrapper.git_repository_free(repo);
            LibGit2Wrapper.git_commit_free(commitPtr);
            LibGit2Wrapper.git_diff_free(diff);
        }

        private static IntPtr AccessRepository()
        {
            var repoPath = FindDirectoryContainingGitFolder(Environment.CurrentDirectory);

            if (repoPath == null)
            {
                Console.WriteLine("\nCould not find a Git repository in any directory.");
            }

            LibGit2Wrapper.git_libgit2_init();
            IntPtr repo = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_repository_open(out repo, repoPath!) != 0)
                {
                    throw new Exception("Failed to open the repository.");
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error: {ex.Message}");
                Console.Write(ex.StackTrace);
            }

            return repo;
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


