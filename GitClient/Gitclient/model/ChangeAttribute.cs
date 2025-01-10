using GitClient.repository;
using GitClient.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.model
{
    public class ChangeAttribute
    {
        private string symbol { get; }
        private string fileName { get; }
        private string filePath { get; }
        

        public ChangeAttribute(string changeType, string fileName, string filePath)
        {
            symbol = changeType;
            this.fileName = fileName;
            this.filePath = filePath;
        }

        public string Display()
        {
            return this.symbol + "    " + this.fileName;
        }

        public string GetSymbol()
        {
            return symbol;
        }

        public string GetFileName()
        {
            return fileName;
        }

        public string GetFilePath()
        {
            return filePath;
        }
    }
}
