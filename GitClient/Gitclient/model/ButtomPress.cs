using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.model
{
    public static class ButtomPress
    {
        public struct Type
        {
            public static bool down;
            public static bool up;
            public static bool deleted;

            public Type() 
            {
                down = false;
                up = false;
                deleted = false;
            }
        }
    }
}
