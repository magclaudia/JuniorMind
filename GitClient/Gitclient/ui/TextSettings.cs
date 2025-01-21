using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitClient.ui
{
    public class TextSettings
    {
        public static string GetTextLength(string text, int width)
        {
            return  text.Length < width ?  text : text.Substring(0, width);
        }

        public static ConsoleColor SetColorStatus(char symbol)
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

        public static void SetColorLog(string text)
        {
            List<string> splitText = Regex.Matches(text, @"\S+\s*").Select(match => match.Value).ToList();
            string message = string.Join(" ", splitText.Skip(3));
            Dictionary<string, ConsoleColor> keyValuePairs = new Dictionary<string, ConsoleColor>();

            keyValuePairs.Add(splitText[0], ConsoleColor.Magenta);
            keyValuePairs.Add(splitText[1], ConsoleColor.Cyan);
            keyValuePairs.Add(splitText[2], ConsoleColor.Green);
            keyValuePairs.Add(message, ConsoleColor.White);

            foreach (var elem in keyValuePairs)
            {
                Console.ForegroundColor = elem.Value;
                Console.Write(elem.Key);
            }

            Console.ResetColor();
        }
    }
}
