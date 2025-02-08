using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;

namespace GitClient.model
{
    public class ButtonStates
    {
        public bool Down { get; set; }
        public bool Up { get; set; }
        public bool Enter { get; set; }
        public bool Right { get; set; }
        public bool Left { get; set; }
        public bool Diff { get; set; }
        public bool GitLog { get; set; }

        public ButtonStates(bool down, bool up, bool enter, bool right, bool left, bool diff, bool gitLog)
        {
            Down = down;
            Up = up;
            Enter = enter;
            Right = right;
            Left = left;
            Diff = diff;
            GitLog = gitLog;
        }
    }
}
