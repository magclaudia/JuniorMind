namespace GitClient
{
    public class Message
    {
        public static void ReturnMessage(int index, CommitElements commitElements, GetVariablesForCommits variablesForCommits)
        {
            string message;
            string outputMessage;
            int firstIndex = 0;
            int lengthForNow = 0;
            int heightForMessage = 0;
            int widthForMessage = 0;
            int xForMessageBox = 0;

            if (variablesForCommits.pressRight == 0)
            {
                var size = new DrawPanelRigthSide.MessageBox();
                heightForMessage = size.height;
                xForMessageBox = size.edgeOne + 1;
                widthForMessage = size.width - 2;
            }
            else
            {
                var size = new DrawPanelLeftSide.MessageBox();
                heightForMessage = size.height;
                xForMessageBox = size.edgeOne + 1;
                widthForMessage = size.width - 2;
            }

            var messageList = new GetCommits.Elements();
            messageList.Message = commitElements.Message[index];
            messageList.Description = commitElements.Description[index];

            if (variablesForCommits.right == true)
            {
                Console.SetCursorPosition(1, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2) + 1);
            }
            else
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 + 11, Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2) + 1);
            }

            if (messageList.Description != "")
            {
                message = $"{messageList.Message}.Description: {messageList.Description}";
            }
            else
            {
                message = messageList.Message;
            }

            for (int i = Console.WindowHeight / 2 - ((Console.WindowHeight / 2) / 2) + 1; i < heightForMessage; i++)
            {
                if (message.Length - lengthForNow > widthForMessage)
                {
                    outputMessage = message.Substring(firstIndex, widthForMessage).TrimStart();
                    firstIndex++;
                }
                else
                {
                    outputMessage = message.Substring(firstIndex, message.Length - lengthForNow);
                }

                lengthForNow += outputMessage.Length;
                if (variablesForCommits.right == true)
                {
                    Console.SetCursorPosition(1, i);
                }
                else
                {
                    Console.SetCursorPosition(xForMessageBox, i);
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
