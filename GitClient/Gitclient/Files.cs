using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace GitClient
{
    public class Files
    {
        public static void GetFilesAffectedByCommit(IntPtr repo, IntPtr commitPtr, int index)
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
            var position = new DrawPanel.FilesBox();
            var options = new LibGit2Wrapper.GitDiffOptions();

            if (LibGit2Wrapper.git_diff_tree_to_tree(out diff, repo, parentTreePtr, treePtr, ref options) != 0)
            {
                throw new Exception("Failed to get the diff.");
            }

            UIntPtr numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);
            Console.SetCursorPosition(position.edgeOneX + 1, position.edgeOneY);
            Console.WriteLine($"Files: {numDeltas} ");

            PrintAllFiles.PrintAllFilesAffectedByCommit(numDeltas, diff, index);
            LibGit2Wrapper.git_diff_free(diff);

            Marshal.FreeHGlobal(options.old_prefix);
            Marshal.FreeHGlobal(options.new_prefix);

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
        }
    }
}
