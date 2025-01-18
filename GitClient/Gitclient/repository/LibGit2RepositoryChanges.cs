using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GitClient.model;

namespace GitClient.repository
{
    public class LibGit2RepositoryChanges
    {
        private LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();

        public List<ChangeAttribute> GetAllUnstagedChanges()
        {
            List<ChangeAttribute> stagedChanges = GetAllStageChanges();
            List<ChangeAttribute> unstagedChanges = new List<ChangeAttribute>();

            IntPtr diff = IntPtr.Zero;
            IntPtr deltaPtr = IntPtr.Zero;

            try
            {
                diff = GetDiff();
                nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);

                if (numDeltas > 0)
                {
                    for (UIntPtr i = 0; i < numDeltas; i++)
                    {
                        deltaPtr = LibGit2Wrapper.git_diff_get_delta(diff, i);

                        if (deltaPtr == IntPtr.Zero)
                        {
                            throw new Exception("Failed to get delta.");
                        }

                        var delta = Marshal.PtrToStructure<LibGit2Wrapper.GitDiffDelta>(deltaPtr);

                        string filePath = Marshal.PtrToStringAnsi(delta.new_file.path)
                                  ?? Marshal.PtrToStringAnsi(delta.old_file.path)
                                  ?? throw new InvalidOperationException("File path is null");

                        string fileName = Path.GetFileName(filePath)!;

                        if (stagedChanges.Any(s => s.GetFilePath() == filePath))
                        {
                            continue;
                        }

                        unstagedChanges.Add(new ChangeAttribute(Symbol(delta), fileName, filePath));
                    }
                }
            }
            finally
            {
                if (diff != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(diff);
                }
            }

            return unstagedChanges;
        }


        public List<ChangeAttribute> GetAllStageChanges()
        {
            List<ChangeAttribute> stagedChanges = new List<ChangeAttribute>();
            IntPtr repo = GetRepo();
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr treePtr = IntPtr.Zero;
            IntPtr index = IntPtr.Zero;
            IntPtr diff = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_reference_name_to_id(out LibGit2Wrapper.GitOid commitOid, repo, "HEAD") != 0)
                {
                    throw new Exception("Failed to get HEAD commitOid.");
                }

                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref commitOid) != 0)
                {
                    throw new Exception("Failed to lookup commit.");
                }

                if (LibGit2Wrapper.git_commit_tree(out treePtr, commitPtr) != 0)
                {
                    throw new Exception("Failed to get the commit tree.");
                }

                if (LibGit2Wrapper.git_repository_index(out index, repo) != 0)
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


                if (LibGit2Wrapper.git_diff_tree_to_index(out diff, repo, treePtr, index, ref options) != 0)
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
                        string filePath = Marshal.PtrToStringAnsi(delta.new_file.path)
                                  ?? Marshal.PtrToStringAnsi(delta.old_file.path)
                                  ?? throw new InvalidOperationException("File path is null");

                        string fileName = Path.GetFileName(filePath)!;
                        stagedChanges.Add(new ChangeAttribute(Symbol(delta), fileName, filePath));
                    }
                }
            }
            finally
            {
                if (repo != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_repository_free(repo);
                }

                if (commitPtr != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_commit_free(commitPtr);
                }

                if (treePtr != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_tree_free(treePtr);
                }

                if (index != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(index);
                }

                if (diff != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(diff);
                }
            }

            return stagedChanges;
        }


        public void StageFile(ChangeAttribute unstagedFile)
        {
            IntPtr repo = GetRepo();
            IntPtr index = IntPtr.Zero;
            string filePath = unstagedFile.GetFilePath();

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
            finally
            {
                if (repo != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_repository_free(repo);
                }

                if (index != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(index);
                }
            }
        }


        public void UnstageFile(ChangeAttribute stagedFile)
        {
            IntPtr repo = GetRepo();
            IntPtr headRef = IntPtr.Zero;
            IntPtr headCommit = IntPtr.Zero;
            string filePath = stagedFile.GetFilePath();
            LibGit2Wrapper.GitStrArray pathspec = new LibGit2Wrapper.GitStrArray();

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

                pathspec = new LibGit2Wrapper.GitStrArray
                {
                    strings = nativeArray,
                    count = 1
                };

                if (LibGit2Wrapper.git_reset_default(repo, headCommit, ref pathspec) != 0)
                {
                    throw new Exception($"Fail to reset file {filePath}.");
                }
            }
            finally
            {
                if (repo != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_repository_free(repo);
                }

                if (headRef != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_reference_free(headRef);
                }

                if (headCommit != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_object_free(headCommit);
                }
            }
        }

        public IntPtr GetDiff()
        {
            IntPtr index = IntPtr.Zero;
            IntPtr diff = IntPtr.Zero;
            IntPtr repo = GetRepo();

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

                options.flags |= (uint)(LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_INCLUDE_UNTRACKED |
                                        LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_RECURSE_UNTRACKED_DIRS |
                                        LibGit2Wrapper.DiffOptionFlags.GIT_DIFF_SHOW_UNTRACKED_CONTENT);

                if (LibGit2Wrapper.git_diff_index_to_workdir(out diff, repo, index, ref options) != 0)
                {
                    throw new Exception("Failed to get diff between tree and workdir for untracked files.");
                }
            }
            finally
            {
                if (index != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(index);
                }
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
