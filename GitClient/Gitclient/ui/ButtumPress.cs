using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public static class ButtomPress
    {
        public struct Type
        {
            public static bool down;
            public static bool up;
            public static bool left;
            public static bool right;
            public static bool deleted;
            public static bool diffMovements;
            public static bool escape;
            public static bool enter;
            public static bool workingInUnstagePanel;
            public static bool workingInStagePanel;

            public Type()
            {
                down = false;
                up = false;
                left = false;
                right = false;
                deleted = false;
                diffMovements = false;
                escape = false;
                enter = false;
                workingInUnstagePanel = false;
                workingInStagePanel = false;
            }
        }
    }
}
