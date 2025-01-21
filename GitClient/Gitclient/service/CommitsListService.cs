
using Gitclient.model;
using GitClient.model;
using GitClient.repository;
using static GitClient.DrawTabs;

namespace GitClient.service
{
    public class CommitsListService
    {
        private CommitsLisLibGit2Repository libgit2Repository; 

        public CommitsListService(CommitsLisLibGit2Repository libgit2Repository) 
        {
            this.libgit2Repository = libgit2Repository;
        }

        public List<CommitsElements> GetAllCommits()
        {
            return libgit2Repository.GetAllCommits();
        }

        public List<CommitsElements> GetCurrentListOfCommits(int startIndex, int endIndex)
        {
            List<CommitsElements> commits = GetAllCommits();
            int maxVisibleChanges = Console.WindowHeight - 4;
            int count = Math.Min(maxVisibleChanges, commits.Count - startIndex);

            if (count + startIndex > commits.Count)
            {
                count--;
            }

            if (commits.Count > 0)
            {
                if (startIndex >= 0 && count > 0)
                {
                    commits = commits.GetRange(startIndex, count);
                }
                else
                {
                    commits = new List<CommitsElements>();
                }
            }

            return commits;
        }
    }
}