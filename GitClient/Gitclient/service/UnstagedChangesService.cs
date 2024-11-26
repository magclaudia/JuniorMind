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
        private List<string> listOfUnstagedFiles = new List<string>();

        public UnstagedChangesService() 
        {

        }

        public string GetUstagedFiles()
        {
            var list = GetFiles.GetListOfAllFiles;
            return "Files 1, File 2";
        }
    }
}
