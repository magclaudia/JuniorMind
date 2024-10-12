using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient
{
    public class StatusTab
    {
        public static void GetUnstagedChanges(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list, GetVariablesForTabs tab)
        {
            IntPtr unstagedDiff = IntPtr.Zero;
            IntPtr indexPtr = IntPtr.Zero;
            var options = new LibGit2Wrapper.GitDiffOptions();

            try
            {
                if (LibGit2Wrapper.git_repository_index(out indexPtr, commitElements.repo) != 0)
                {
                    throw new Exception("Failed to get the repository index");
                }


                if (LibGit2Wrapper.git_diff_index_to_workdir(out unstagedDiff, commitElements.repo, indexPtr, ref options) != 0)
                {
                    throw new Exception("Failed to get unstaged changes");
                }

                nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(unstagedDiff);

                if (numDeltas == 0)
                {
                    Console.SetCursorPosition(1, 3);
                    string text = Tabs.SetTabTextLength("No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                }

                int index = 0;
                tab.unstageChanges = true;
                GetAllFiles.PrintAllFiles(commitElements.repo, numDeltas, unstagedDiff, index, variablesForCommits, list, commitElements, tab);
                GetDiffForChanges.DiffUnstagedChanges(unstagedDiff, list, variablesForFiles, variablesForCommits, tab);
                variablesForFiles.indexDiff = 0;
                variablesForFiles.fileIndex = 0;
               
                tab.unstageChanges = false;
                //list.ClearAllLists();
            }
            finally 
            {
                if (unstagedDiff != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(unstagedDiff);
                }

                if (indexPtr != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(indexPtr);
                }
            }
        }

        public static void GetStagedChanges(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetCertainList list, GetVariablesForTabs tab)
        {
            LibGit2Wrapper.GitOid commitOid;
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr treePtr = IntPtr.Zero; 
            IntPtr indexPtr = IntPtr.Zero;  
            IntPtr stagedDiff = IntPtr.Zero; 
            var options = new LibGit2Wrapper.GitDiffOptions();

            try
            {
                if (LibGit2Wrapper.git_reference_name_to_id(out commitOid, commitElements.repo, "HEAD") != 0)
                {
                    throw new Exception("Failed to get HEAD commitOid");
                }

                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, commitElements.repo, ref commitOid) != 0)
                {
                    throw new Exception("Failed to lookup commit");
                }

                if (LibGit2Wrapper.git_commit_tree(out treePtr, commitPtr) != 0)
                {
                    throw new Exception("Failed to get the commit tree");
                }

                if (LibGit2Wrapper.git_repository_index(out indexPtr, commitElements.repo) != 0)
                {
                    throw new Exception("Failed to get the repository index");
                }

                if (LibGit2Wrapper.git_diff_tree_to_index(out stagedDiff, commitElements.repo, treePtr, indexPtr, ref options) != 0)
                {
                    throw new Exception("Failed to get diff between tree and index");
                }

                int index = 0;
                nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(stagedDiff);
                if (numDeltas == 0)
                {
                    Console.SetCursorPosition(1, Console.WindowHeight / 2 + 3);
                    string text = Tabs.SetTabTextLength("No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                }

                tab.stageChanges = true;
                GetAllFiles.PrintAllFiles(commitElements.repo, numDeltas, stagedDiff, index, variablesForCommits, list, commitElements, tab);
                tab.stageChanges = false;
                //list.ClearAllLists();
            }
            finally
            {
                if (stagedDiff != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(stagedDiff); 
                }

                if (indexPtr != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(indexPtr); 
                }

                if (treePtr != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_tree_free(treePtr);
                }

                if (commitPtr != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_commit_free(commitPtr);
                }
            }
            
        }

        public static void GetStatusChangesNames()
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            string unstaged = SetStatusTabTextLength("Unstaged Changes: ");
            Console.SetCursorPosition(1, dimensions.tabHeight + 1);
            Console.Write(unstaged);
            
            string staged = SetStatusTabTextLength("Staged Changes: ");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetCursorPosition(1, dimensions.height / 2 + 2);
            }
            else
            {
                Console.SetCursorPosition(1, dimensions.height / 2 + 2);
            }

            Console.Write(staged);
            
            string diff = "Diff: ";
            Console.SetCursorPosition(Console.WindowWidth / 2 + 1, dimensions.tabHeight + 1);
            Console.Write(diff);
        }

        private static string SetStatusTabTextLength(string text)
        {
            string outputText;
            if (text.Length < Console.WindowWidth / 2)
            {
                outputText = text;
            }
            else
            {
                outputText = text.Substring(0, Console.WindowWidth / 2);
            }

            return outputText;
        }
    }
}
