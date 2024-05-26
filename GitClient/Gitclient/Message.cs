using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient
{
    public class Message
    {
        public static void ReturnMessage(List<string> listOfCommits, int index)
        {
            string message = "";
            int firstIndex = 0;
            int comparationLength = 0;
            int borderWidths = 2;
            int leftBorderWidth = 1;
            int standardColumnsWidth = 30;
            string completeMessage = listOfCommits[index][standardColumnsWidth..];

            for (int i = 1; i < Console.WindowHeight / 2 - borderWidths; i++)
            {
                if (completeMessage.Length - comparationLength > Console.WindowWidth / 2 - borderWidths)
                {
                    message = completeMessage.Substring(firstIndex, Console.WindowWidth / 2 - borderWidths);
                    firstIndex++;
                }
                else
                {
                    message = completeMessage.Substring(firstIndex, completeMessage.Length -  comparationLength);
                }

                comparationLength += message.Length;
                Console.SetCursorPosition(Console.WindowWidth / 2 + leftBorderWidth, i);
                Console.Write(message);
                firstIndex += message.Length - 1;

                if (comparationLength == completeMessage.Length)
                {
                    break;
                }
            }
        }
    }
}
