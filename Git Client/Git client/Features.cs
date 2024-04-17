using System;


namespace GitClient
{
    public class Features
    {
        public static void DisplayFeatures()
        {
            var command = Console.ReadLine();
            while (command != "e")
            {
                if (command == "1")
                {
                    AccessRepository();
                    break;
                }

                Console.WriteLine("\nYou entered a wrong command, please try again.");
                command = Console.ReadLine();
                if (command == "e")
                {
                    break;
                }
            }

            Console.WriteLine("\nThe application is closed.");
        }

        private static void AccessRepository()
        {
            Console.WriteLine("\nPlease add your repository path here, and then press \"ENTER\":\n");
            var repoPath = Console.ReadLine();
            
            LibGit2Wrapper.git_libgit2_init();
            
            bool repoFound = false;
            IntPtr repo = IntPtr.Zero;
            while (repoFound == false)
            {
                if (repoPath == "e")
                {
                    break;
                }
                
                try
                {
                    if (LibGit2Wrapper.git_repository_open(out repo, repoPath) != 0)
                    {
                        throw new Exception("Fail to open the repository.");
                    }

                    ListOfCommits.GetAllCommits(repo);
                    repoFound = true;
                }
                catch
                {
                    Console.WriteLine("\nFailed to open the repository.Please input a new path.");
                    repoPath = Console.ReadLine();
                }
            }

            LibGit2Wrapper.git_repository_free(repo);
        }
    }
}
