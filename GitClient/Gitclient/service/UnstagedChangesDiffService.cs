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
        private readonly LibGit2UnstagedDiffRepository repository;

        public UnstagedChangesDiffService(LibGit2UnstagedDiffRepository libGit2UnstagedDiff)
        {
            this.repository = libGit2UnstagedDiff;
        }

        public List<Diff> GetAllUnstagedDiff()
        {
            return repository.GetAllUnstagedDiff();
        }

        public List<Diff> GetCurrentDiff(int currentIndex)
        {
            List<Diff> diff = repository.GetAllUnstagedDiff();
            return diff.GetRange(currentIndex, 1);
        }
    }
}
