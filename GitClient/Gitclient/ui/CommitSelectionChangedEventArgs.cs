using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using GitClient.repository;
using GitClient.service;
using GitClient.ui;

namespace GitClient.ui
{
    public class CommitSelectionChangedEventArgs : EventArgs
    {
        public int StartIndex { get; }
        public int EndIndex { get; }
        public int CurrentIndex { get; }
        public int FileIndex { get; set; }
        public bool Enter { get; }
        public bool Right { get; }
        public bool Left { get; }
        public bool Diff { get; }
        public int Y { get; set; }
        public int CommitNumber { get; set; }
        public int FileNumber { get; set; }
        public int FilesStartingIndex { get; set; }
        public int DiffIndex { get; set; }
        public int DiffStartingindex { get; set; }

        public CommitSelectionChangedEventArgs(bool enter, bool right, bool left, bool diff, int startIndex, int endIndex, int currentIndex, int fileIndex, int y, int commitNumber, int fileNumber, int filesStartingIndex, int diffIndex, int diffStartingindex)
        {
            Enter = enter;
            Right = right;
            Left = left;
            Diff = diff;
            StartIndex = startIndex;
            EndIndex = endIndex;
            CurrentIndex = currentIndex;
            FileIndex = fileIndex;
            Y = y;
            CommitNumber = commitNumber;
            FileNumber = fileNumber;
            FilesStartingIndex = filesStartingIndex;
            DiffIndex = diffIndex;
            DiffStartingindex = diffStartingindex;
        }
    }
}
