using GitClient.model;
using GitClient.repository;
using LibGit2Sharp;
using static GitClient.DrawTabs;

namespace GitClient.service
{
    public class CommitsService
    {
        private CommitsLibGit2Repository libgit2Repository; 

        public CommitsService(CommitsLibGit2Repository libgit2Repository) 
        {
            this.libgit2Repository = libgit2Repository;
        }


        public List<model.Commit> GetAllCommits()
        {
            return libgit2Repository.GetAllCommits();
        }

        public List<model.Commit> GetCurrentListOfCommits(int startIndex, int endIndex)
        {
            List<model.Commit> commits = GetAllCommits();
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
                    commits = new List<model.Commit>();
                }
            }

            return commits;
        }

        public List<ChangeAttribute> GetAllFilesForCommit(int index)
        {
            return libgit2Repository.GetAllFilesForCommit(index);
        }

        public List<ChangeAttribute> GetCurrentFiles(int startIndex, int index)
        {
            List<ChangeAttribute> filesList = GetAllFilesForCommit(index);
            int maxVisibleChanges = Console.WindowHeight - (Console.WindowHeight / 2) - 6;
            int count = Math.Min(maxVisibleChanges, filesList.Count - startIndex);

            if (count + startIndex > filesList.Count)
            {
                count--;
            }

            if (filesList.Count > 0)
            {
                if (startIndex >= 0 && count > 0)
                {
                    filesList = filesList.GetRange(startIndex, count);
                }
                else
                {
                    filesList = new List<ChangeAttribute>();
                }
            }

            return filesList;

        }

        public List<FileDiff> GetAllLogDiff(int index)
        {
            return libgit2Repository.GetAllDiffs(index);
        }

        public List<string> GetCurrentDiffForSelectedFile(int index, string fileName) 
        {
            return libgit2Repository.GetCurrentDiffForSelectedFile(index, fileName);
        }
    }
}