using GitClient.ui;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.model
{
    public class FileDiff
    { 
        public List<string> diffs { get; set; }
        public string fileName { get; set; }

        public FileDiff(List<string> diffs, string fileName)
        {
            this.diffs = diffs;
            this.fileName = fileName;
        }
    }
}
