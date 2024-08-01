using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class Message
    {
        public static void ReturnMessage(int index, CommitElements commitElements, VariablesForCommits indexes)
        {
            string message;
            string outputMessage;
            int firstIndex = 0;
            int lengthForNow = 0;
            var size = new DrawPanelRigthSide.MessageBox();
            var messageList = new GetCommits.Elements();
            messageList.Message = commitElements.Message[index];
            messageList.Description = commitElements.Description[index];
            
            if (indexes.rigth == true)
            {
                Console.SetCursorPosition(1, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2) + 1);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2) + 1);
            }

            if (messageList.Description != "")
            {
                message = $"{messageList.Message}.Description: {messageList.Description}"; ;
            }
            else
            {
                message = messageList.Message;
            }

            for (int i = Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2) + 1; i < size.height; i++)
            {
                if (message.Length - lengthForNow > Console.WindowWidth / 2 - 10 - 2)
                {
                    outputMessage = message.Substring(firstIndex, size.width).TrimStart();
                    firstIndex++;
                }
                else
                {
                    outputMessage = message.Substring(firstIndex, message.Length - lengthForNow);
                }

                lengthForNow += outputMessage.Length;
                if (indexes.rigth == true)
                {
                    Console.SetCursorPosition(1, i);
                }
                else
                {
                    Console.SetCursorPosition(size.edgeOne + 1, i);
                }

                Console.Write(outputMessage);
                firstIndex += outputMessage.Length - 1;
                if (lengthForNow == message.Length)
                {
                    break;
                }
            }
        }
    }
}
