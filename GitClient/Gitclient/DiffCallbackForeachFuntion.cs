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
        public static GetVariablesForFiles files = new GetVariablesForFiles();
        public static GetCertainList list1 = new GetCertainList();
        public static GetVariablesForCommits variablesForCommit = new GetVariablesForCommits();

        public static int ReturnForeachCallback(IntPtr diff, GetCertainList list, GetVariablesForFiles variablesForFiles, GetVariablesForCommits variablesForCommits)
        {
            files = variablesForFiles;
            list1 = list;
            variablesForCommit = variablesForCommits;

            int i = 0;
            var filesList = new List<string>();

            if (variablesForCommit.logTab == true)
            {
                filesList = list.listOfFiles;

                while (i < filesList.Count)
                {
                    list.listOfAllDiffs.Add(new List<string>());
                    list.startingIndexesLog.Add(new List<int>());
                    i++;
                }
            }
            else
            {
                if (variablesForFiles.unstageChanges == true)
                {
                    filesList = list.unstagedChangesFiles;

                    while (i < filesList.Count)
                    {
                        list.unstagedChangesDiff.Add(new List<string>());
                        list.unstagedDiffListStartAt.Add(new List<int>());
                        i++;
                    }
                }
            }

            return LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);
        }

        public static int DiffFileCallback(ref LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string fileName = string.Empty;
            
            if (variablesForCommit.logTab == true)
            {
                fileName = list1.filesNames[files.fileIndex];
            }
            else 
            {
                if (files.unstageChanges == true)
                {
                    fileName = list1.unstagedChangesFiles[files.fileIndex];
                    fileName = fileName.Remove(0, 5);
                }
                else
                {
                    fileName = list1.stagedChangesFiles[files.fileIndex];
                    fileName = fileName.Remove(0, 5);
                }
            }

            string text = newFilePath!;
           
            if (newFilePath!.Contains(fileName))
            {
                files.indexDiff++;
                if (variablesForCommit.logTab == true)
                {
                    list1.listOfAllDiffs[files.indexDiff].Add(text);
                    list1.startingIndexesLog[files.indexDiff].Add(0);
                }
                else
                {
                    if (files.unstageChanges == true)
                    {
                        list1.unstagedChangesDiff[files.indexDiff].Add(text);
                        list1.unstagedDiffListStartAt[files.fileIndex].Add(0);
                    }
                    else
                    {
                        list1.stagedChangesDiff[files.indexDiff].Add(text);
                        list1.stagedDiffListStartAt[files.fileIndex].Add(0);
                    }
                }
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

            if (variablesForCommit.logTab == true)
            {
                list1.listOfAllDiffs[files.indexDiff].Add(text);
            }
            else
            {
                if (files.unstageChanges == true)
                {
                    list1.unstagedChangesDiff[files.indexDiff].Add(text);
                }
                else
                {
                    list1.stagedChangesDiff[files.indexDiff].Add(text);
                }
            }

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

            if (variablesForCommit.logTab == true)
            {
                list1.listOfAllDiffs[files.indexDiff].Add(text);
                files.nextFile = true;
            }
            else
            {
                if (files.unstageChanges == true)
                {
                    list1.unstagedChangesDiff[files.indexDiff].Add(text);
                }
                else
                {
                    list1.stagedChangesDiff[files.indexDiff].Add(text);
                }
            }

            return 0;
        }
    }
}
