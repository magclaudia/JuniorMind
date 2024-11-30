using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.model
{
    public class UnstagedChange
    {
        private ChangeType changeType;
        private string fileName;
        private string path;

        public UnstagedChange(ChangeType changeType, string fileName, string path)
        {
            this.path = path;
            this.changeType = changeType;
            this.fileName = fileName;
        }

        public string toDisplay()
        {
            return this.fileName + " " + path + " " + changeType;
        }
    }
}
