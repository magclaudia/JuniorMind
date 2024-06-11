using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class Message
    {
        public static void ReturnMessage(List<string> listOfCommits, int index, string timeDateLength)
        {
            string message;
            int firstIndex = 0;
            int lengthForNow = 0;
            int borders = 2;
            int standardColumnsWidthIfTime = 28;
            int standardColumnsWidthIfDate = 30;
            const int timeStandardLength = 8;
            string completeMessage;
            var size = new DrawPanel.MessageBox();
            
            if (timeDateLength.Length == timeStandardLength)
            {
                completeMessage = listOfCommits[index][standardColumnsWidthIfTime..];
            }
            else
            {
                completeMessage = listOfCommits[index][standardColumnsWidthIfDate..];
            }

            
            for (int i = 1; i < size.height; i++)
            {
                if (completeMessage.Length - lengthForNow > Console.WindowWidth / 2 - 10 - borders)
                {
                    message = completeMessage.Substring(firstIndex, size.width).TrimStart();
                    firstIndex++;
                }
                else
                {
                    message = completeMessage.Substring(firstIndex, completeMessage.Length - lengthForNow - 1);
                }

                lengthForNow += message.Length;
                Console.SetCursorPosition(size.edgeOne + 1, i);
                Console.Write(message);
                firstIndex += message.Length - 1;

                if (lengthForNow == completeMessage.Length - 1)
                {
                    break;
                }
            }
        }
    }
}
