using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class Info
    {
        public static void GetInfo(Indexes indexes, CommitElements listOfCommits)
        {
            var position = new DrawPanel.InfoPanel();
            string[] infos = { "Author", "Date/Time", "Sha" };
            string author = listOfCommits.Author[indexes.currentCommitIndex];
            string dateOrTime = listOfCommits.DateTime[indexes.currentCommitIndex];
            string sha = listOfCommits.Id[indexes.currentCommitIndex];
            string[] arrayOfInfos = { author, dateOrTime, sha };
            string output;
            for (int i = 1; i < position.height; i++)
            {
                Console.SetCursorPosition(position.edgeOne + 1, i);
                output = ($"{infos[i - 1]}: {arrayOfInfos[i - 1]}");
                if (output.Length > position.width)
                {
                    output = output.Substring(0, position.width);
                }
                else
                {
                    output = output.Substring(0, output.Length);
                }

                Console.Write(output);

                if (i == 3)
                {
                    break;
                }
            }
        }
    }
}
