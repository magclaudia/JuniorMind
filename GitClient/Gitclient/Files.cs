using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient
{
    public class Files
    {
        public static void GetFilesAffectedByCommit(IntPtr repo, IntPtr commitPtr)
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

            IntPtr diffPtr = IntPtr.Zero;
            if (LibGit2Wrapper.git_diff_tree_to_tree(out diffPtr, repo, parentTreePtr, treePtr, IntPtr.Zero) != 0)
            {
                throw new Exception("Failed to get the diff.");
            }

            int numDeltas = LibGit2Wrapper.git_diff_num_deltas(diffPtr);
            for (int i = 0; i < numDeltas; i++)
            {
                IntPtr deltaPtr = LibGit2Wrapper.git_diff_get_delta(diffPtr, i);
                var delta = Marshal.PtrToStructure<GitDiffDelta>(deltaPtr);

                string oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
                string newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);

                switch (delta.status)
                {
                    case GitDelta.GIT_DELTA_ADDED:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"+  {newFilePath}");
                        break;
                    case GitDelta.GIT_DELTA_MODIFIED:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"M  {newFilePath}");
                        break;
                    case GitDelta.GIT_DELTA_DELETED:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"-  {oldFilePath}");
                        break;
                }

                Console.ResetColor();
            }

            LibGit2Wrapper.git_diff_free(diffPtr);

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


