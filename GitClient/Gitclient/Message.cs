using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class Message
    {
        public static void ReturnMessage(int index, ListOfCommits.CommitElements commitElements)
        {
            string message;
            int firstIndex = 0;
            int lengthForNow = 0;
            var size = new DrawPanel.MessageBox();
            var messageList = new Commits.Elements();

            messageList.Message = commitElements.Message[index];
            for (int i = 1; i < size.height; i++)
            {
                if (messageList.Message.Length - lengthForNow > Console.WindowWidth / 2 - 10 - 2)
                {
                    message = messageList.Message.Substring(firstIndex, size.width).TrimStart();
                    firstIndex++;
                }
                else
                {
                    message = messageList.Message.Substring(firstIndex, messageList.Message.Length - lengthForNow - 1);
                }

                lengthForNow += message.Length;
                Console.SetCursorPosition(size.edgeOne + 1, i);
                Console.Write(message);
                firstIndex += message.Length - 1;

                if (lengthForNow == messageList.Message.Length - 1)
                {
                    break;
                }
            }
        }
    }
}
