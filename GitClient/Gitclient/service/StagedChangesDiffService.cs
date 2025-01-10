using GitClient.model;
using GitClient.ui;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.service
{
    public class StagedChangesDiffService
    {
        //private List<Diffs> listOfStageDiffs;

        //public StagedChangesDiffService()
        //{
        //    listOfStageDiffs = new List<Diffs>();
        //}

        //public List<Diffs> GetAllStageDiffs()
        //{
        //    return listOfStageDiffs;
        //}

        //public void AddStageDiffs(Diffs diff) 
        //{
        //    listOfStageDiffs.Add(diff);
        //}

        //public List<Diffs> GetCurrentStageDiff(int currentIndex) 
        //{
        //    List<Diffs> diff = GetAllStageDiffs();

        //    if (diff.Count() > 0)
        //    {
        //        return diff.GetRange(currentIndex, 1);
        //    }

        //    return diff;
        //}

    }
}
