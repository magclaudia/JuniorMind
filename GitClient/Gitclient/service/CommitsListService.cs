
using Gitclient.model;
using GitClient.repository;

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
    }
}