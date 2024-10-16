using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class DiffCallbackForeachFuntion
    {
        private static GetVariablesForFiles files = new GetVariablesForFiles();
        private static GetCertainList list1 = new GetCertainList();
        private static GetVariablesForTabs tab1 = new GetVariablesForTabs();    
        private static GetVariablesForCommits variablesForCommit = new GetVariablesForCommits();

        public static int ReturnForeachCallback(IntPtr diff, GetCertainList list, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits, GetVariablesForTabs tab)
        {
            files = variablesForFiles;
            list1 = list;
            tab1 = tab;
            variablesForCommit = variablesForCommits;

            int i = 0;
            var filesList = new List<string>();
            if (variablesForCommit.logTab == true)
            {
                filesList = list.listOfFiles;
            }
            else
            {
                if (variablesForFiles.unstageChanges == true)
                {
                    filesList = list.unstagedChangesFiles;
                }
                else
                {
                    filesList = list.stagedChangesFiles;
                }
            }

            while (i < filesList.Count)
            {
                list.listOfAllDiffs.Add(new List<string>());
                list.stagedChangesDiff.Add(new List<string>());
                list.unstagedChangesDiff.Add(new List<string>());
                list.startingIndexesLog.Add(new List<int>());
                list.startingIndexesTab.Add(new List<int>());
                i++;
            }

            return LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);
        }

        public static int DiffFileCallback(ref LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string fileName = "";
            
            if (variablesForCommit.logTab == true)
            {
                fileName = list1.filesNames[files.fileIndex];
            }
            else 
            {
                fileName = list1.unstagedChangesFiles[files.fileIndex];
                fileName = fileName.Remove(0, 5);
            }

            string text = newFilePath!;
           
            if (newFilePath!.Contains(fileName))
            {
                files.indexDiff++;
                list1.listOfAllDiffs[files.indexDiff].Add(text);
                list1.startingIndexesLog[files.indexDiff].Add(0);
            }

            files.fileIndex++;
            return 0;
        }

        public static int DiffBinaryCallback(ref LibGit2Wrapper.GitDiffDelta delta, IntPtr binary, IntPtr payload)
        {
            return 0;
        }

        public static int DiffHunkCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, IntPtr payload)
        {
            byte[] filteredHeader = hunk.header.Where(c => c != '\0' && c != '0').ToArray();
            string hunkHeader = System.Text.Encoding.UTF8.GetString(filteredHeader);
            string text = hunkHeader;
            
            if (text.Contains('\n'))
            {
                text = text.Remove(text.IndexOf('\n'));
            }

            list1.listOfAllDiffs[files.indexDiff].Add(text);
            return 0;
        }

        public static int DiffLineCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, ref LibGit2Wrapper.GitDiffLine line, IntPtr payload)
        {
            string content = Marshal.PtrToStringAnsi(line.content, (int)line.content_len);
            
            if (content.StartsWith('\t'))
            {
                string output = content.Replace("\t", new string(' ', 4));
                content = output + content;
            }

            if (content.Contains("\n\t"))
            {
                content = content[..content.IndexOf("\n\t")];
            }

            string text = $"{(char)line.origin} {content}";

            if (list1.listOfAllDiffs[files.indexDiff][files.index].StartsWith("=")
               || list1.listOfAllDiffs[files.indexDiff][files.index].StartsWith("<")
                 || list1.listOfAllDiffs[files.indexDiff][files.index].StartsWith(">"))
            {
                list1.listOfAllDiffs[files.indexDiff].RemoveAt(list1.listOfAllDiffs[files.indexDiff].IndexOf("="));
                list1.listOfAllDiffs[files.indexDiff].RemoveAt(list1.listOfAllDiffs[files.indexDiff].IndexOf("<"));
                list1.listOfAllDiffs[files.indexDiff].RemoveAt(list1.listOfAllDiffs[files.indexDiff].IndexOf(">"));
            }

            list1.listOfAllDiffs[files.indexDiff].Add(text);
            files.nextFile = true;
            return 0;
        }
    }
}
