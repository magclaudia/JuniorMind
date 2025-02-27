using GitClient.model;
using GitClient.service;
using GitClient.ui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.repository
{
    public class LibGit2RepositoryDiff
    {
        private LibGit2RepositoryChanges libGit2RepositoryChanges;
        private LibGit2Wrapper.GitDiffOptions options;
        private List<FileDiff> unstagedDiff;
        private List<FileDiff> stageDiffs;
        private Dictionary<string, List<string>> keyValuePairs;

        public LibGit2RepositoryDiff(LibGit2RepositoryChanges libGit2RepositoryChanges)
        {
            this.libGit2RepositoryChanges = libGit2RepositoryChanges;
            options = new LibGit2Wrapper.GitDiffOptions();
            unstagedDiff = new List<FileDiff>();
            stageDiffs = new List<FileDiff>();
            keyValuePairs = new Dictionary<string, List<string>>();
        }

        public List<FileDiff> GetAllUnstagedDiff(IntPtr diff)
        {
            var stagedDiff = GetAllStageDiffs();

            if (diff == IntPtr.Zero)
            {
                throw new Exception("Failed to retrieve a valid diff object.");
            }

            nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);
            long testVariable = (long)numDeltas;

            keyValuePairs.Clear();
            unstagedDiff.Clear();

            if (numDeltas != 0)
            {
                int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

                if (result != 0)
                {
                    throw new Exception("Failed to iterate over diff.");
                }
            }

            foreach (var entry in keyValuePairs)
            {
                //if (stagedDiff.Any(s => s.fileName == entry.Key))
                //{
                //    continue;
                //}

                unstagedDiff.Add(new FileDiff(entry.Value, entry.Key));
            }

            return unstagedDiff;
        }

        public List<FileDiff> GetAllStageDiffs()
        {
            IntPtr repo = libGit2RepositoryChanges.GetRepo();
            IntPtr diff = IntPtr.Zero;
            IntPtr index = IntPtr.Zero;
            IntPtr headCommit = IntPtr.Zero;
            IntPtr tree = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_repository_index(out index, repo) != 0)
                {
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_reference_name_to_id(out var oid, repo, "HEAD") != 0)
                {
                    throw new Exception("Failed to resolve HEAD reference.");
                }

                if (LibGit2Wrapper.git_commit_lookup(out headCommit, repo, ref oid) != 0)
                {
                    throw new Exception("Failed to lookup HEAD commit.");
                }

                if (LibGit2Wrapper.git_commit_tree(out tree, headCommit) != 0)
                {
                    throw new Exception("Failed to get the repository tree.");
                }

                if (LibGit2Wrapper.git_diff_tree_to_index(out diff, repo, tree, index, ref options) != 0)
                {
                    throw new Exception("Failed to get the repository diff.");
                }

                keyValuePairs.Clear();
                stageDiffs.Clear();

                int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

                if (result != 0)
                {
                    throw new Exception("Failed to iterate over diff.");
                }

                foreach (var entry in keyValuePairs)
                {
                    stageDiffs.Add(new FileDiff(entry.Value, entry.Key));
                }
            }
            finally
            {
                if (repo != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_repository_free(repo);
                }

                if (diff != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_diff_free(diff);
                }

                if (index != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_index_free(index);
                }

                if (headCommit != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_object_free(headCommit); 
                }

                if (tree != IntPtr.Zero)
                {
                    LibGit2Wrapper.git_tree_free(tree);
                }
            }

            return stageDiffs;
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
            byte[] contentBytes = new byte[(int)line.content_len];
            Marshal.Copy(line.content, contentBytes, 0, (int)line.content_len);
            string content = Encoding.UTF8.GetString(contentBytes);
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

            string text = $"{(char)line.origin}{content}".TrimEnd();

            if (!string.IsNullOrEmpty(filePath) && keyValuePairs.ContainsKey(filePath))
            {
                if (text != "=\n\\ No newline at end of file")
                {
                    keyValuePairs[filePath].Add(text);
                }
            }

            return 0;
        }
    }
}
