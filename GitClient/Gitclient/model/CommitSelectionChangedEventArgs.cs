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
        public ButtonStates ButtonStates { get; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public int CommitIndex { get; set; }
        public int FileIndex { get; set; }
        public int Y { get; set; }
        public int CommitNumber { get; set; }
        public int FileNumber { get; set; }
        public int FilesStartingIndex { get; set; }
        public int DiffIndex { get; set; }
        public int DiffStartingIndex { get; set; }

        public CommitSelectionChangedEventArgs(ButtonStates buttonStates, int startIndex, int endIndex, int currentIndex,
            int fileIndex, int y, int commitNumber, int fileNumber, int filesStartingIndex, int diffIndex, int diffStartingIndex)
        {
            ButtonStates = buttonStates;
            StartIndex = startIndex;
            EndIndex = endIndex;
            CommitIndex = currentIndex;
            FileIndex = fileIndex;
            Y = y;
            CommitNumber = commitNumber;
            FileNumber = fileNumber;
            FilesStartingIndex = filesStartingIndex;
            DiffIndex = diffIndex;
            DiffStartingIndex = diffStartingIndex;
        }
    }
}
