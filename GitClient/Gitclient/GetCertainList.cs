using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class GetCertainList
    {
        public  List<string> listOfFiles;
        public  List<string> filesNames;
        public  List<string> diffForEachFile;
        public  List<List<string>> listOfDiffsForEachFiles;
        public  List<string> listOfDiff;
        public  List<string> filePath;
        public  List<string> hunks;
        public  List<string> filesCode;
        public  List<LibGit2Wrapper.GitDiffLineOrigin> origin;
        public  List<string> addLines;

        public GetCertainList() 
        {
            listOfFiles = new List<string>();
            filesNames = new List<string>();
            listOfDiff = new List<string>();
            diffForEachFile = new List<string>();
            listOfDiffsForEachFiles = new List<List<string>>();
            filePath = new List<string>();
            hunks = new List<string>();
            filesCode = new List<string>();
            origin = new List<LibGit2Wrapper.GitDiffLineOrigin>();
            addLines = new List<string>();
        }
    }
}
