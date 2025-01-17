using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;
using static GitClient.LibGit2Wrapper;

namespace GitClient.repository
{
    public class LibGit2Repository
    {
        private LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();

        public List<ChangeAttribute> GetAllUnstagedChanges()
        {
            List<ChangeAttribute> unstagedChanges = new List<ChangeAttribute>();
            IntPtr diff = GetDiff();

            nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);

            if (numDeltas > 0)
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
                    unstagedChanges.Add(new ChangeAttribute(Symbol(delta), fileName, filePath));
                }
            }

            return unstagedChanges;
        }


        public List<ChangeAttribute> GetAllStageChanges()
        {
            List<ChangeAttribute> stagedChanges = new List<ChangeAttribute>();
            IntPtr repo = GetRepo();

            try
            {
                if (LibGit2Wrapper.git_reference_name_to_id(out LibGit2Wrapper.GitOid commitOid, repo, "HEAD") != 0)
                {
                    throw new Exception("Failed to get HEAD commitOid.");
                }

                if (LibGit2Wrapper.git_commit_lookup(out IntPtr commitPtr, repo, ref commitOid) != 0)
                {
                    throw new Exception("Failed to lookup commit.");
                }

                if (LibGit2Wrapper.git_commit_tree(out IntPtr treePtr, commitPtr) != 0)
                {
                    throw new Exception("Failed to get the commit tree.");
                }

                if (LibGit2Wrapper.git_repository_index(out IntPtr index, repo) != 0)
                {
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_index_read(index, 0) != 0)
                {
                    throw new Exception("Failed to read index.");
                }

                options.flags |= (uint)(LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_INCLUDE_UNTRACKED |
                                       LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_RECURSE_UNTRACKED_DIRS |
                                       LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_SHOW_UNTRACKED_CONTENT);


                if (LibGit2Wrapper.git_diff_tree_to_index(out IntPtr diff, repo, treePtr, index, ref options) != 0)
                {
                    throw new Exception("Failed to create diff.");
                }

                nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);

                if (numDeltas > 0)
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
                        string filePath = newFilePath ?? oldFilePath ?? throw new InvalidOperationException("Both file paths are null");

                        string fileName = Path.GetFileName(filePath)!;
                        stagedChanges.Add(new ChangeAttribute(Symbol(delta), fileName, filePath));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting staged changes: {ex.Message}");
            }

            return stagedChanges;
        }


        public void StageFile(ChangeAttribute unstagedFile)
        {
            IntPtr repo = GetRepo();
            string filePath = unstagedFile.GetFilePath();
            IntPtr index = IntPtr.Zero;
           
            try
            {
                if (LibGit2Wrapper.git_repository_index(out index, repo) != 0)
                {
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_index_read(index, 0) != 0)
                {
                    throw new Exception("Failed to read index.");
                }

                if (unstagedFile.GetSymbol() == "-")
                {
                    if (LibGit2Wrapper.git_index_remove_bypath(index, filePath) != 0)
                    {
                        throw new Exception($"Failed to remove deleted file from the repository index: {filePath}");
                    }
                }
                else
                {
                    if (LibGit2Wrapper.git_index_add_bypath(index, filePath) != 0)
                    {
                        throw new Exception($"Failed to add file to the repository index: {filePath}");
                    }
                }

                if (LibGit2Wrapper.git_index_write(index) != 0)
                {
                    throw new Exception("Failed to write the repository index.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting file: {ex.Message}");
            }
            finally
            {
                if (index != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(index);
                }
            }
        }


        public void UnstageFile(ChangeAttribute stagedFile)
        {
            IntPtr repo = GetRepo();
            string filePath = stagedFile.GetFilePath();
            IntPtr headRef = IntPtr.Zero;
            IntPtr headCommit = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_repository_head(out headRef, repo) != 0)
                {
                    throw new Exception("Failed to get HEAD reference.");
                }

                if (LibGit2Wrapper.git_reference_peel(out headCommit, headRef, LibGit2Wrapper.GitObjectType.GIT_OBJECT_COMMIT) != 0)
                {
                    throw new Exception("Failed to resolve HEAD to commit.");
                }

                string[] filePaths = { filePath };
                IntPtr[] strArray = new IntPtr[filePaths.Length];

                for (int i = 0; i < filePaths.Length; i++)
                {
                    strArray[i] = Marshal.StringToCoTaskMemAnsi(filePaths[i]);
                }

                IntPtr nativeArray = Marshal.AllocCoTaskMem(IntPtr.Size * strArray.Length);
                Marshal.Copy(strArray, 0, nativeArray, strArray.Length);

                var pathspec = new GitStrArray
                {
                    strings = nativeArray,
                    count = 1
                };

                if (LibGit2Wrapper.git_reset_default(repo, headCommit, ref pathspec) != 0)
                {
                    throw new Exception($"Fail to reset file {filePath}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting file: {ex.Message}");
            }
            finally
            {
                // Free the repository and target commit pointers
                //if (repo != IntPtr.Zero)
                //{
                //    LibGit2Wrapper.git_repository_free(repo);
                //}

                //if (targetCommit != IntPtr.Zero)
                //{
                //    Marshal.FreeCoTaskMem(targetCommit);
                //}
            }

        }
      

        public IntPtr GetDiff()
        {
            IntPtr index = IntPtr.Zero;
            IntPtr headTree = IntPtr.Zero;
            IntPtr repo = GetRepo();
            IntPtr diff = IntPtr.Zero;
            IntPtr headRef = IntPtr.Zero;
            IntPtr headCommit = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_repository_index(out index, repo) != 0)
                {
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_index_read(index, 0) != 0)
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

                //options.flags |= (uint)(LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_INCLUDE_UNTRACKED |
                //                        LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_RECURSE_UNTRACKED_DIRS |
                //                        LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_SHOW_UNTRACKED_CONTENT);

                if (LibGit2Wrapper.git_diff_index_to_workdir(out diff, repo, index, ref options) != 0)
                {
                    throw new Exception("Failed to get diff between tree and workdir for untracked files.");
                }
            }
            finally
            {
                //if (index != IntPtr.Zero)
                //{
                //    LibGit2Wrapper.git_index_free(index);
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

        public IntPtr GetRepo()
        {
            GetProjectPath projectPath = new GetProjectPath();
            var repoPath = projectPath.ProjectPath(Environment.CurrentDirectory);
            IntPtr repo = IntPtr.Zero;

            if (repoPath == null)
            {
                Console.WriteLine("\nCould not find a Git repository in any directory.");
            }

            LibGit2Wrapper.git_libgit2_init();

            if (LibGit2Wrapper.git_repository_open(out repo, repoPath!) != 0)
            {
                throw new Exception("Failed to open the repository.");
            }

            return repo;
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
    }
}
