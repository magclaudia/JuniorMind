using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.ui
{
    public class ClearConsoleChoosenSpace
    {
        public void Clear(int curentIndex, int x, int y, int startFrom, int width, int EndAt)
        {
            if (y == EndAt)
            {
                while (startFrom <= EndAt)
                {
                    Console.SetCursorPosition(x, startFrom);
                    Console.Write(new string(' ', width));
                    startFrom++;
                }
            }
            else
            {
                Console.SetCursorPosition(x, y);
                Console.Write(new string(' ', width));
            }
        }
    }
}
