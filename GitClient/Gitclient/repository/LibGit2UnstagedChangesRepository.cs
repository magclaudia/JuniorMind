using Gitclient.model;
using GitClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.repository
{
    public class LibGit2UnstagedChangesRepository
    {
        private LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();
        private DrawTabs.Dimensions dimensions = new DrawTabs.Dimensions();

        public List<UnstagedChange> GetAllUnstagedChanges()
        {
            List<UnstagedChange> unstagedChanges = new List<UnstagedChange>();
            IntPtr diff = IntPtr.Zero; 

            diff = GetDiff();
            nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);
            
            if (numDeltas == 0)
            {
                Console.SetCursorPosition(1, dimensions.unstagedStart);
                string text = Tabs.SetStatusTextLength(" No changes found in the unstaging area.", Console.WindowWidth / 2 - 3);
                Console.WriteLine(text);
            }
            else
            {
                for (UIntPtr i = 0; i < numDeltas; i++)
                {
                    IntPtr deltaPtr = LibGit2Wrapper.git_diff_get_delta(diff, i);

                    if (deltaPtr == IntPtr.Zero)
                    {
                        throw new Exception("Failed to get delta.");
                    }

                    var delta = Marshal.PtrToStructure<LibGit2Wrapper.GitDiffDelta>(deltaPtr);
                    string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
                    string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
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
                    unstagedChanges.Add(new UnstagedChange(Symbol(delta), fileName));
                }
            }

            return unstagedChanges;
        }

        public IntPtr GetDiff()
        {
            IntPtr indexPtr = IntPtr.Zero;
            IntPtr headRef = IntPtr.Zero;
            IntPtr headCommit = IntPtr.Zero;
            IntPtr headTree = IntPtr.Zero;
            IntPtr repo = GetRepo();
            IntPtr diff = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_repository_index(out indexPtr, repo) != 0)
                {
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_index_read(indexPtr, 0) != 0)
                {
                    throw new Exception("Failed to read index.");
                }

                if (LibGit2Wrapper.git_repository_head(out headRef, repo) != 0)
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

                if (LibGit2Wrapper.git_diff_tree_to_workdir_with_index(out diff, repo, headTree, ref options) != 0)
                {
                    throw new Exception("Failed to get diff between tree and workdir for untracked files.");
                }
            }
            finally
            {
                //if (indexPtr != IntPtr.Zero)
                //{
                //    LibGit2Wrapper.git_index_free(indexPtr);
                //}

                //if (headRef != IntPtr.Zero)
                //{
                //    LibGit2Wrapper.git_reference_free(headRef);
                //}

                //if (headCommit != IntPtr.Zero)
                //{
                //    LibGit2Wrapper.git_tree_free(headCommit);
                //}

                //if (headTree != IntPtr.Zero)
                //{
                //    LibGit2Wrapper.git_diff_free(headTree);
                //}

                //if (diff != IntPtr.Zero)
                //{
                //    LibGit2Wrapper.git_diff_free(diff);
                //}

                //LibGit2Wrapper.git_repository_free(repo);
            }

            return diff;
        }

        private string Symbol(LibGit2Wrapper.GitDiffDelta delta)
        {
            return delta.status switch
            {
                LibGit2Wrapper.GitDelta.GIT_DELTA_UNTRACKED => "+",
                LibGit2Wrapper.GitDelta.GIT_DELTA_ADDED => "+",
                LibGit2Wrapper.GitDelta.GIT_DELTA_MODIFIED => "M",
                LibGit2Wrapper.GitDelta.GIT_DELTA_DELETED => "-",
                _ => throw new NotImplementedException()
            };
        }

        private IntPtr GetRepo()
        {
            GetProjectPath projectPath = new GetProjectPath();
            var repoPath = projectPath.ProjectPath(Environment.CurrentDirectory);
            IntPtr repo = IntPtr.Zero;

            if (repoPath == null)
            {
                Console.WriteLine("\nCould not find a Git repository in any directory.");
            }

            LibGit2Wrapper.git_libgit2_init();

            try
            {
                if (LibGit2Wrapper.git_repository_open(out repo, repoPath!) != 0)
                {
                    throw new Exception("Failed to open the repository.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            return repo;
        }
    }
}
