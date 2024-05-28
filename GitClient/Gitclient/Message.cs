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
        public static void ReturnMessage(List<string> listOfCommits, int index, string timeDateLength)
        {
            string message;
            int firstIndex = 0;
            int lengthForNow = 0;
            int borderWidths = 2;
            int leftBorderWidth = 1;
            int standardColumnsWidthIfTime = 28;
            int standardColumnsWidthIfDate = 30;
            const int timeStandardLength = 8;
            string completeMessage;
            
            
            if (timeDateLength.Length == timeStandardLength)
            {
                completeMessage = listOfCommits[index][standardColumnsWidthIfTime..];
            }
            else
            {
                completeMessage = listOfCommits[index][standardColumnsWidthIfDate..];
            }

            for (int i = 1; i < Console.WindowHeight / 2 - borderWidths; i++)
            {
                if (completeMessage.Length - lengthForNow > Console.WindowWidth / 2 - 10 - borderWidths)
                {
                    message = completeMessage.Substring(firstIndex, Console.WindowWidth - Console.WindowWidth / 2  - 10 - borderWidths).TrimStart();
                    firstIndex++;
                }
                else
                {
                    message = completeMessage.Substring(firstIndex, completeMessage.Length - lengthForNow - 1);
                }

                lengthForNow += message.Length;
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, i);
                Console.Write(message);
                firstIndex += message.Length - 1;

                if (lengthForNow == completeMessage.Length)
                {
                    break;
                }
            }
        }
    }
}
