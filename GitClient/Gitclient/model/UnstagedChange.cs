using Gitclient.repository;
using Gitclient.ui;
using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.model
{
    public class UnstagedChange
    {
        public string symbol;
        public string fileName;

        public UnstagedChange(string changeType, string fileName)
        {
            this.symbol = changeType;
            this.fileName = fileName;
        }

        public string Display()
        {
            return this.symbol + "    " + this.fileName;
        }
    }
}
