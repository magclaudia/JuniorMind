using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using GitClient.repository;
using GitClient.ui;

namespace GitClient.service
{
    public class StatusDiffService
    {
        private readonly UnstageLibGit2DiffRepository unstageDiffRepository;
        private readonly StageLibGit2DiffRepository stageDiffRepository;
        private readonly LibGit2Repository libGit2Repository;
        private static int indexForDiff;

        public StatusDiffService(UnstageLibGit2DiffRepository unstageDiffRepositoryDiff, StageLibGit2DiffRepository stageDiffRepository, LibGit2Repository libGit2Repository)
        {
            this.unstageDiffRepository = unstageDiffRepositoryDiff;
            this.libGit2Repository = libGit2Repository;
            this.stageDiffRepository = stageDiffRepository;
        }

        public int GetDiffIndex(int diffIndex)
        {
            return indexForDiff;
        }

        public List<Diffs> GetAllStageDiffs()
        {
            return stageDiffRepository.GetAllStageDiffs();
        }

        public List<Diffs> GetCurrentStageDiff(int currentIndex)
        {
            List<Diffs> diff = GetAllStageDiffs();

            if (diff.Count() > 0)
            {
                return diff.GetRange(currentIndex, 1);
            }

            return diff;
        }

        public List<Diffs> GetAllUnstageDiffs()
        {
            return unstageDiffRepository.GetAllUnstagedDiff(libGit2Repository.GetDiff());
        }

        public List<Diffs> GetCurrentUnstageDiff(int currentIndex)
        {
            List<Diffs> diff = unstageDiffRepository.GetAllUnstagedDiff(libGit2Repository.GetDiff());
            
            if (diff.Count() > 0)
            {
                return diff.GetRange(currentIndex, 1);
            }

            return diff;
        }
    }
}
