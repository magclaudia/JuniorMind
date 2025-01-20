using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.repository;
using GitClient.service;

namespace GitClient.ui
{
    public class CommitsPanel : UiComponent
    {
        private CommitsLisLibGit2Repository libgit2Repository;
        private CommitsListService commitsListService;

        public CommitsPanel()
        {
            libgit2Repository = new CommitsLisLibGit2Repository();
            commitsListService = new CommitsListService(libgit2Repository);
        }

        public override void Show()
        {

        }
    }
}
