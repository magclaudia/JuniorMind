namespace GitClient
{
    public class GetStatusFiles
    {
        public static void GetUnstagedChanges(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();

            IntPtr diff = IntPtr.Zero;
            IntPtr indexPtr = IntPtr.Zero;
            IntPtr headRef = IntPtr.Zero;
            IntPtr headCommit = IntPtr.Zero;
            IntPtr headTree = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_repository_index(out indexPtr, commitElements.repo) != 0)
                {
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_index_read(indexPtr, 0) != 0)
                {
                    throw new Exception("Failed to read index.");
                }

                if (LibGit2Wrapper.git_repository_head(out headRef, commitElements.repo) != 0)
                {
                    throw new Exception("Failed to get HEAD reference.");
                }

                if (LibGit2Wrapper.git_reference_peel(out headCommit, headRef, LibGit2Wrapper.GitObjectType.GIT_OBJECT_COMMIT) != 0)
                {
                    throw new Exception("Failed to resolve HEAD to commit.");
                }

                if (LibGit2Wrapper.git_commit_tree(out headTree, headCommit) != 0)
                {
                    throw new Exception("Failed to get tree from HEAD commit.");
                }

                options.flags |= (uint)(LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_INCLUDE_UNTRACKED |
                                        LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_RECURSE_UNTRACKED_DIRS |
                                        LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_SHOW_UNTRACKED_CONTENT);

                if (LibGit2Wrapper.git_diff_tree_to_workdir_with_index(out diff, commitElements.repo, headTree, ref options) != 0)
                {
                    throw new Exception("Failed to get diff between tree and workdir for untracked files.");
                }

                nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);

                if (numDeltas == 0)
                {
                    Console.SetCursorPosition(1, dimensions.unstagedStart);
                    string text = Tabs.SetStatusTextLength(" No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                }
                else
                {
                    int index = 0;
                    variablesForFiles.unstageChanges = true;
                    GetFiles.GetListOfAllFiles(commitElements.repo, numDeltas, diff, index, variablesForCommits, variablesForFiles, list, commitElements);
                    GetDiffForChanges.DiffUnstagedChanges(diff, list, variablesForFiles, variablesForCommits);
                    variablesForFiles.indexDiff = 0;
                    variablesForFiles.fileIndex = 0;
                }
            }
            finally
            {
                if (indexPtr != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(indexPtr);
                }

                if (headRef != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_reference_free(headRef);
                }

                if (headCommit != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_tree_free(headCommit);
                }

                if (headTree != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(headTree);
                }

                if (diff != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(diff);
                }
            }
        }

        public static void GetStagedChanges(CommitElements commitElements, GetVariablesForCommits variablesForCommits, GetVariablesForFiles variablesForFiles, GetCertainList list)
        {
            LibGit2Wrapper.GitOid commitOid;
            DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();
            LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();
            
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr treePtr = IntPtr.Zero;
            IntPtr indexPtr = IntPtr.Zero;
            IntPtr stagedDiff = IntPtr.Zero;

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
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_index_read(indexPtr, 0) != 0)
                {
                    throw new Exception("Failed to read index.");
                }

                if (LibGit2Wrapper.git_diff_tree_to_index(out stagedDiff, commitElements.repo, treePtr, indexPtr, ref options) != 0)
                {
                    throw new Exception("Failed to create diff.");
                }

                int index = 0;
                UIntPtr numDeltas = LibGit2Wrapper.git_diff_num_deltas(stagedDiff);

                if (numDeltas == 0)
                {
                    Console.SetCursorPosition(1, dimensions.stagedStart);
                    string text = Tabs.SetStatusTextLength("No changes found in the staging area.", Console.WindowWidth / 2 - 3);
                    Console.WriteLine(text);
                }
                else
                {
                    variablesForFiles.stageChanges = true;
                    variablesForFiles.unstageChanges = false;
                    GetFiles.GetListOfAllFiles(commitElements.repo, numDeltas, stagedDiff, index, variablesForCommits, variablesForFiles, list, commitElements);
                    GetDiffForChanges.DiffUnstagedChanges(stagedDiff, list, variablesForFiles, variablesForCommits);
                    variablesForFiles.indexDiff = 0;
                    variablesForFiles.fileIndex = 0;

                    if (list.unstagedChangesFiles.Count > 0)
                    {
                        variablesForFiles.unstageChanges = true;
                        variablesForFiles.stageChanges = false;
                    }
                }
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
            Console.SetCursorPosition(1, dimensions.stagedStart - 2);
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
