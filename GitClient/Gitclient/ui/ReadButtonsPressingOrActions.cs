using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public static class ReadButtonsPressingOrActions
    {
        public struct Type
        {
            public static bool down;
            public static bool up;
            public static bool left;
            public static bool rightOnce;
            public static bool rightTwice;
            public static bool rightStatus;
            public static bool enter;

            public static bool deleted;
            public static bool diffMovements;
            public static bool workingInUnstagePanel;
            public static bool workingInStagePanel;
            public static bool displayListOfCommitsOnEntirePanel;
        }
    }
}
