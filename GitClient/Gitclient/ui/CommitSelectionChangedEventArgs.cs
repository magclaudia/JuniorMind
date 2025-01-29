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
        public bool Enter { get; }
        public bool Right { get; }
        public bool Left { get; }
        public bool Diff { get; }
        public int Y { get; set; }

        public int CommitNumber { get; set; }

        public CommitSelectionChangedEventArgs(bool enter, bool right, bool left, bool diff, int startIndex, int endIndex, int currentIndex, int y, int commitNumber)
        {
            Enter = enter;
            Right = right;
            Left = left;
            Diff = diff;
            StartIndex = startIndex;
            EndIndex = endIndex;
            CurrentIndex = currentIndex;
            Y = y;
            CommitNumber = commitNumber;
        }
    }
}
