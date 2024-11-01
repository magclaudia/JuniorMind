using System.Runtime.InteropServices;

namespace GitClient
{
    public class Files
    {
        public static void GetFilesAffectedByCommit(IntPtr repo, IntPtr commitPtr, int index, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, CommitElements commitElements)
        {
            IntPtr parentCommitPtr = IntPtr.Zero;
            IntPtr parentTreePtr = IntPtr.Zero;
            IntPtr treePtr = IntPtr.Zero;
            GetVariablesForTabs tab = new GetVariablesForTabs();

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
            var position = new DrawPanelRigthSide.FilesBox();
            var options = new LibGit2Wrapper.GitDiffOptions();

            if (LibGit2Wrapper.git_diff_tree_to_tree(out diff, repo, parentTreePtr, treePtr, ref options) != 0)
            {
                throw new Exception("Failed to get the diff.");
            }

            if (variablesForFiles.nextFile == false && variablesForCommits.pressRight < 2)
            {
                UIntPtr numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);

                if (variablesForCommits.right == true)
                {
                    Console.SetCursorPosition(1, position.edgeOneY);
                }
                else
                {
                    Console.SetCursorPosition(position.edgeOneX + 1, position.edgeOneY);
                }

                Console.WriteLine($"Files: {numDeltas} ");
                GetAllFiles.GetListOfAllFiles(repo, numDeltas, diff, index, variablesForCommits, variablesForFiles, list, commitElements, tab);
                GetAllFiles.PrintFilesForLog(list, variablesForCommits, variablesForFiles, tab);
                if (variablesForCommits.right == true)
                {
                    GetVariablesForFiles variablesForFile = new GetVariablesForFiles();
                    GetDiffs.GetFileContent(diff, variablesForFile, variablesForCommits, commitElements, list);
                }
            }
            else
            {
                GetDiffs.GetFileContent(diff, variablesForFiles, variablesForCommits, commitElements, list);
            }

            LibGit2Wrapper.git_diff_free(diff);

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
        }
    }
}
