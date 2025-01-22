using GitClient.model;
using GitClient.service;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace GitClient.repository
{
    public class CommitsLibGit2Repository
    {
        LibGit2Wrapper.GitOid id = new LibGit2Wrapper.GitOid();
        List<LibGit2Wrapper.GitOid> oids = new List<LibGit2Wrapper.GitOid>();

        public List<CommitsElements> GetAllCommits()
        {
            List<CommitsElements> listOFCommits = new List<CommitsElements>();
            IntPtr walker = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr repo = GetRepo();
            string message = "";

            try
            {
                if (LibGit2Wrapper.git_revwalk_new(out walker, repo) != 0)
                {
                    throw new Exception("Could not create revision walker");
                }

                if (LibGit2Wrapper.git_revwalk_push_head(walker) != 0)
                {
                    throw new Exception("Could not find repository HEAD");
                }

                while (LibGit2Wrapper.git_revwalk_next(out id, walker) == 0)
                {
                    if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref id) == 0)
                    {
                        string commitMessage = Marshal.PtrToStringAnsi(LibGit2Wrapper.git_commit_message(commitPtr))!;
                        string[] messageParts = commitMessage.Split(new[] { '\n' }, 2);

                        if (messageParts[0].Contains("\n\n"))
                        {
                            int index = messageParts[0].IndexOf('\n');
                            message = messageParts[0].Remove(index);
                        }
                        else
                        {
                            message = messageParts[0].TrimEnd();
                        }

                        string description;

                        if (messageParts.Length > 1)
                        {
                            description = messageParts[1].Trim();
                        }
                        else
                        {
                            description = string.Empty;
                        }

                        listOFCommits.Add(new CommitsElements(GetCommitId(id), GetDateAndTime(commitPtr), GetCommitAuthor(commitPtr), message));
                    }
                    else
                    {
                        throw new Exception("Fail to look up the commit.");
                    }

                    oids.Add(id);
                }
            }
            finally
            {
                LibGit2Wrapper.git_commit_free(commitPtr);
                LibGit2Wrapper.git_revwalk_free(walker);
            }

            return listOFCommits;
        }

        public List<ChangeAttribute> GetAllFilesForCommit(int index)
        {
            List<ChangeAttribute> filesList = new List<ChangeAttribute>();

            LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();

            IntPtr parentCommitPtr = IntPtr.Zero;
            IntPtr parentTreePtr = IntPtr.Zero;
            IntPtr treePtr = IntPtr.Zero;
            IntPtr diff = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr deltaPtr = IntPtr.Zero;
            IntPtr repo = GetRepo();
            id = oids[index];

            try
            {
                if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref id) != 0)
                {
                    throw new Exception("Fail to lookup the commit.");
                }

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

                if (LibGit2Wrapper.git_diff_tree_to_tree(out diff, repo, parentTreePtr, treePtr, ref options) != 0)
                {
                    throw new Exception("Failed to get the diff.");
                }

                UIntPtr numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);
               
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

                        filesList.Add(new ChangeAttribute(Symbol(delta), fileName, filePath));
                    }
                }
            }
            finally
            {
                if (diff != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(diff);
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
            }

            return filesList;
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

        private string GetCommitId(LibGit2Wrapper.GitOid id)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in id.Id)
            {
                sb.Append(b.ToString("x2"));
            }

            var commitIdFullLine = sb.ToString();
            return $"{commitIdFullLine.Remove(7)}";
        }

        private string GetCommitAuthor(IntPtr commit)
        {
            IntPtr signaturePtr = LibGit2Wrapper.git_commit_author(commit);
            if (signaturePtr == IntPtr.Zero)
            {
                throw new Exception("Author information could not be retrieved for the given commit.");
            }

            string authorName = "";
            string commitAuthor;
            try
            {
                authorName = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(signaturePtr))!;
            }
            finally
            {
                commitAuthor = $"{authorName}";
            }

            return commitAuthor;
        }

        private string GetDateAndTime(IntPtr commit)
        {
            var date = DateTimeOffset.FromUnixTimeSeconds(LibGit2Wrapper.git_commit_time(commit));
            var adjustedDate = date.ToOffset(new TimeSpan(3, 0, 0));

            if (adjustedDate.Date == DateTimeOffset.UtcNow.Date)
            {
                return adjustedDate.ToString("HH:mm:ss");
            }
            else
            {
                return adjustedDate.ToString("yyyy-MM-dd");
            }
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