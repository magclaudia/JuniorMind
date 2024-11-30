using Gitclient.model;
using Gitclient.repository;
using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public class UnstagedChangesService

        // in constructor initializez atributul lista de mai jos cu toate changes, venite de la api
    // tine un atribut List de usntaged changes si le incarca pe toate din API
    // ai metoda getCurrentUnstagedChanges care returneaza din lista cu toate, pe alea intee start si end index
    // o sa tine repo

    {
        private LibGit2Repository libGit2Repository;


        public UnstagedChangesService(LibGit2Repository libGit2Repository) 
        {
            this.libGit2Repository = libGit2Repository;
        }

        public List<UnstagedChange> GetCurrentUnstagedChanges(int startIndex, int endIndex)
        {
            // map and return between indexes
            return libGit2Repository.getAllUnstagedChanges();
        }
    }
}
