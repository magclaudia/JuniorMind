using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Gitclient.model;
using Gitclient.repository;
using Gitclient.ui;
using GitClient.ui;

namespace Gitclient.service
{
    public class UnstagedChangesDiffService
    {
        private readonly LibGit2UnstagedDiffRepository repositoryDiff;
        private readonly LibGit2UnstagedChangesRepository repositoryChanges;

        public UnstagedChangesDiffService(LibGit2UnstagedDiffRepository repositoryDiff, LibGit2UnstagedChangesRepository repositoryChanges)
        {
            this.repositoryDiff = repositoryDiff;
            this.repositoryChanges = repositoryChanges;
        }

        public List<Diff> GetAllUnstagedDiff()
        {
            return repositoryDiff.GetAllUnstagedDiff(repositoryChanges.GetDiff());
        }

        public List<Diff> GetCurrentDiff(int currentIndex)
        {
            List<Diff> diff = repositoryDiff.GetAllUnstagedDiff(repositoryChanges.GetDiff());
            return diff.GetRange(currentIndex, 1);
        }
    }
}
