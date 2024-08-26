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
        public  List<int> startingIndexes;
        public  List<string> listOfDiff;
        public  List<string> filePath;
        public  List<string> hunks;
        public  List<string> filesCode;
        public  List<string> addLines;
        public List<int> listStartAt;
        public List<int> start;
        public List<string> addList;

        public GetCertainList() 
        {
            listOfFiles = new List<string>();
            filesNames = new List<string>();
            listOfDiff = new List<string>();
            startingIndexes = new List<int>();
            filePath = new List<string>();
            hunks = new List<string>();
            filesCode = new List<string>();
            addLines = new List<string>();
            listStartAt = new List<int>();
            start = new List<int>();
            addList = new List<string>();
        }
    }
}
