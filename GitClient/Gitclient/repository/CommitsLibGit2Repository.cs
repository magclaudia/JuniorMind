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
        private IntPtr diff = IntPtr.Zero;
        private Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();


        public List<CommitsElements> GetAllCommits()
        {
            List<CommitsElements> listOFCommits = new List<CommitsElements>();
            IntPtr walker = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr repo = GetRepo();
            string message;
            oids.Clear();

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
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr deltaPtr = IntPtr.Zero;
            IntPtr diffPtr = IntPtr.Zero;
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

        public List<FileDiff> GetAllDiffs(int index)
        {
            keyValuePairs.Clear();
            List<FileDiff> fileDiffs = new List<FileDiff>();
            IntPtr diff = GetDiff(index);

            int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

            if (result != 0)
            {
                throw new Exception("Failed to iterate over diff.");
            }

            foreach (var entry in keyValuePairs)
            {
                fileDiffs.Add(new FileDiff(entry.Value, entry.Key));
            }

            return fileDiffs;
        }

        public List<string> GetCurrentDiffForSelectedFile(int index, string fileName)
        {
            List<FileDiff> diff = new List<FileDiff>();
            List<string> list = new List<string>();
            diff = GetAllDiffs(index);

            foreach (var entry in diff)
            {
                if (entry.fileName == fileName)
                {
                    list = entry.diffs;
                    break;
                }
            }

            return list;
        }

        private int DiffFileCallback(ref LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            string text = "";
            string? filePath = Path.GetFileName(Marshal.PtrToStringAnsi(delta.new_file.path));

            if (string.IsNullOrEmpty(oldFilePath) && string.IsNullOrEmpty(newFilePath))
            {
                throw new Exception("Paths are null or empty.");
            }

            if (!string.IsNullOrEmpty(newFilePath))
            {
                text = newFilePath;
            }

            if (!string.IsNullOrEmpty(filePath) && !keyValuePairs.ContainsKey(filePath))
            {
                keyValuePairs[filePath] = [text];
            }

            return 0;
        }

        private int DiffBinaryCallback(ref LibGit2Wrapper.GitDiffDelta delta, IntPtr binary, IntPtr payload)
        {
            return 0;
        }

        private int DiffHunkCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, IntPtr payload)
        {
            byte[] filteredHeader = hunk.header.Where(c => c != '\0' && c != '0').ToArray();
            string hunkHeader = System.Text.Encoding.UTF8.GetString(filteredHeader);
            string text = hunkHeader;
            string? filePath = Path.GetFileName(Marshal.PtrToStringAnsi(delta.new_file.path));

            if (text.Contains('\n'))
            {
                text = text.Remove(text.IndexOf('\n'));
            }

            if (!string.IsNullOrEmpty(filePath) && keyValuePairs.ContainsKey(filePath))
            {
                keyValuePairs[filePath].Add(text);
            }

            return 0;
        }

        private int DiffLineCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, ref LibGit2Wrapper.GitDiffLine line, IntPtr payload)
        {
            string content = Marshal.PtrToStringAnsi(line.content, (int)line.content_len);
            string? filePath = Path.GetFileName(Marshal.PtrToStringAnsi(delta.new_file.path));

            if (content.StartsWith('\t'))
            {
                string output = content.Replace("\t", new string(' ', 4));
                content = output + content;
            }

            if (content.Contains("\n\t"))
            {
                content = content[..content.IndexOf("\n\t")];
            }

            string text = $"{(char)line.origin} {content}".TrimEnd();

            if (!string.IsNullOrEmpty(filePath) && keyValuePairs.ContainsKey(filePath))
            {
                keyValuePairs[filePath].Add(text);
            }

            return 0;
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

        public IntPtr GetDiff(int index)
        {
            LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();
            IntPtr parentCommitPtr = IntPtr.Zero;
            IntPtr parentTreePtr = IntPtr.Zero;
            IntPtr treePtr = IntPtr.Zero;
            IntPtr diff = IntPtr.Zero;
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr repo = GetRepo();

            //if (LibGit2Wrapper.git_reference_name_to_id(out var oid, repo, "HEAD") != 0)
            //{
            //    throw new Exception("Failed to resolve HEAD reference.");
            //}

            id = oids[index];

            if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref id) != 0)
            {
                throw new Exception("Failed to lookup HEAD commit.");
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

            return diff;
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