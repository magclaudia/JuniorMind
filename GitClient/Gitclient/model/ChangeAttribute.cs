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
        private string Symbol { get; }
        private string FileName { get; }
        private string FilePath { get; }

        

        public ChangeAttribute(string changeType, string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
            Symbol = changeType;
        }

        public string Display()
        {
            return this.Symbol + "    " + this.FileName;
        }

        public string GetSymbol()
        {
            return Symbol;
        }

        public string GetFileName()
        {
            return FileName;
        }

        public string GetFilePath()
        {
            return FilePath;
        }
    }
}
