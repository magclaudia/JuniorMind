using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gitclient.ui
{
    public class TextSettings
    {
        public static string GetTextLength(string text, int width)
        {
            return  text.Length < width ?  text : text.Substring(0, width);
        }

        public static ConsoleColor SetColor(char symbol)
        {
            return symbol switch
            {
                '+' => ConsoleColor.Green,
                '-' => ConsoleColor.Red,
                'M' => ConsoleColor.Yellow,
                '@' => ConsoleColor.DarkBlue,
                _ => ConsoleColor.White
            };
        }
    }
}
