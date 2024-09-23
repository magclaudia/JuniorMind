using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Testing_git_diff_foreach_API
{
    public class GetFiles
    {
        public static IntPtr GetFilesAffectedByCommit(IntPtr repo, IntPtr commitPtr, Get_CommitElement.Elements element)
        {
            IntPtr parentCommitPtr = IntPtr.Zero;
            IntPtr parentTreePtr = IntPtr.Zero;
            IntPtr treePtr = IntPtr.Zero;

            if (LibGit2Wrapper.git_commit_tree(out treePtr, commitPtr) != 0)
            {
                throw new Exception("Failed to get the commit tree.");
            }

            if (LibGit2Wrapper.git_commit_parentcount(commitPtr) > 0)
            {
                if (LibGit2Wrapper.git_commit_parent(out parentCommitPtr, commitPtr, 0) != 0)
                {
                    throw new Exception("Failed to get the parent commit.");
                }

                if (LibGit2Wrapper.git_commit_tree(out parentTreePtr, parentCommitPtr) != 0)
                {
                    throw new Exception("Failed to get the parent commit tree.");
                }
            }

            IntPtr diff = IntPtr.Zero;
            var options = new LibGit2Wrapper.GitDiffOptions();

            if (LibGit2Wrapper.git_diff_tree_to_tree(out diff, repo, parentTreePtr, treePtr, ref options) != 0)
            {
                throw new Exception("Failed to get the diff.");
            }

            UIntPtr numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);
            Console.Write("Files:");
            Console.WriteLine();

            for (UIntPtr i = 0; i < numDeltas.ToUInt64(); i++) 
            {
                IntPtr deltaPtr = LibGit2Wrapper.git_diff_get_delta(diff, i);
                if (deltaPtr == IntPtr.Zero)
                {
                    throw new Exception("Failed to get delta.");
                }

                var delta = Marshal.PtrToStructure<LibGit2Wrapper.GitDiffDelta>(deltaPtr);

                string? oldFilePath = Marshal.PtrToStringUTF8(delta.old_file.path);
                string? newFilePath = Marshal.PtrToStringUTF8(delta.new_file.path);
                string filePath;

                if (newFilePath != null)
                {
                    filePath = newFilePath;
                }
                else if (oldFilePath != null)
                {
                    filePath = oldFilePath!;
                }
                else
                {
                    throw new InvalidOperationException("Both file paths are null");
                }

                string fileName = Path.GetFileName(filePath)!;
                string fileWithSymbol;

                switch (delta.status)
                {
                    case LibGit2Wrapper.GitDelta.GIT_DELTA_ADDED:
                        fileWithSymbol = $"+    {fileName}";
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(fileWithSymbol);
                        Console.WriteLine();
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED:
                        fileWithSymbol = $"M    {fileName}";
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(fileWithSymbol);
                        Console.WriteLine();
                        break;

                    case LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED:
                        fileWithSymbol = $"-    {fileName}";
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(fileWithSymbol);
                        Console.WriteLine();
                        break;
                }

                Console.ResetColor();
            }

            Marshal.FreeCoTaskMem(options.old_prefix);
            Marshal.FreeCoTaskMem(options.new_prefix);

            if (commitPtr != IntPtr.Zero)
            {
                LibGit2Wrapper.git_commit_free(commitPtr);
            }

            if (treePtr != IntPtr.Zero)
            {
                LibGit2Wrapper.git_tree_free(treePtr);
            }

            if (parentTreePtr != IntPtr.Zero)
            {
                LibGit2Wrapper.git_tree_free(parentTreePtr);
            }

            if (parentCommitPtr != IntPtr.Zero)
            {
                LibGit2Wrapper.git_commit_free(parentCommitPtr);
            }

            return diff;
        }
    }
}
