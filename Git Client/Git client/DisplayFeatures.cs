using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class DisplayFeatures
    {
        public static void DisplayTotalNumberOfCommits()
        {
            var command = Console.ReadLine();
            while (command != "e")
            {
                if (command == "1")
                {
                    AccessRepository();
                    break;
                }

                Console.WriteLine("You enter a invalid command please try again.");
                command = Console.ReadLine();
                if (command == "e")
                {
                    break;
                }
            }
        }

        private static void AccessRepository()
        {
            Console.WriteLine("\nPlease add your repository path here, and then press \"ENTER\":");
            var repositoryPath = Console.ReadLine();

            bool repoFound = false;
            while (repoFound == false)
            {
                if (repositoryPath == "e")
                {
                    break;
                }
                
                try
                {
                    var repo = new Repository(repositoryPath);
                    repoFound = true;
                    int commitsCount = 0;
                    foreach (var commit in repo.Commits)
                    {
                        commitsCount++;
                    }

                    Console.WriteLine($"Total number of commits from the given repository is: {commitsCount}");
                }
                catch
                {
                    Console.WriteLine("The repository was not found. Please input a new path.");
                    repositoryPath = Console.ReadLine();
                }
            }
        }
    }
}
