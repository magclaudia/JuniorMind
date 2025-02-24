using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.ui
{
    public static class ReadButtons
    {
        public static bool Down { get; set; }
        public static bool Up { get; set; }
        public static bool Left { get; set; }
        public static bool RightOnce { get; set; }
        public static bool RightStatus { get; set; }
        public static bool Enter { get; set; }
        public static bool Esc { get; set; }

        public static bool Deleted { get; set; }
        public static bool DiffMovements { get; set; }
        public static bool WorkingInUnstagePanel { get; set; }
        public static bool WorkingInStagePanel { get; set; }
        public static bool DisplayListOfCommitsOnEntirePanel { get; set; }
    }
}
