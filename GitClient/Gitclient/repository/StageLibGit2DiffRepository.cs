using GitClient.model;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.repository
{
    public class StageLibGit2DiffRepository
    {
        private LibGit2RepositoryChanges libGit2Repository;
        private LibGit2Wrapper.GitDiffOptions options;
        private List<FileDiff> stageDiffs;
        private Dictionary<string, List<string>> eachFileDiff;

        public StageLibGit2DiffRepository(LibGit2RepositoryChanges libGit2Repository)
        {
            this.libGit2Repository = libGit2Repository;
            options = new LibGit2Wrapper.GitDiffOptions();
            stageDiffs = new List<FileDiff>();
            eachFileDiff = new Dictionary<string, List<string>>();
        }

        public List<FileDiff> GetAllStageDiffs()
        {
            IntPtr repo = libGit2Repository.GetRepo();
            IntPtr diff = IntPtr.Zero;

            try
            {
                if (LibGit2Wrapper.git_repository_index(out IntPtr index, repo) != 0)
                {
                    throw new Exception("Failed to get the repository index.");
                }

                if (LibGit2Wrapper.git_reference_name_to_id(out var oid, repo, "HEAD") != 0)
                {
                    throw new Exception("Failed to resolve HEAD reference.");
                }

                if (LibGit2Wrapper.git_commit_lookup(out IntPtr headCommit, repo, ref oid) != 0)
                {
                    throw new Exception("Failed to lookup HEAD commit.");
                }

                if (LibGit2Wrapper.git_commit_tree(out IntPtr tree, headCommit) != 0)
                {
                    throw new Exception("Failed to get the repository tree.");
                }

                if (LibGit2Wrapper.git_diff_tree_to_index(out diff, repo, tree, index, ref options) != 0)
                {
                    throw new Exception("Failed to get the repository diff.");
                }


            }
            finally
            {

            }

            int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

            if (result != 0)
            {
                throw new Exception("Failed to iterate over diff.");
            }

            foreach (var entry in eachFileDiff)
            {
                stageDiffs.Add(new FileDiff(entry.Value, entry.Key));
            }

            return stageDiffs;
        }

        private int DiffFileCallback(ref LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            string text = "";
            string? fileName = Path.GetFileName(Marshal.PtrToStringAnsi(delta.new_file.path));

            if (string.IsNullOrEmpty(oldFilePath) && string.IsNullOrEmpty(newFilePath))
            {
                throw new Exception("Paths are null or empty.");
            }

            if (!string.IsNullOrEmpty(newFilePath))
            {
                text = newFilePath;
            }

            if (!string.IsNullOrEmpty(fileName) && !eachFileDiff.ContainsKey(fileName))
            {
                eachFileDiff[fileName] = [text];
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
            string? fileName = Path.GetFileName(Marshal.PtrToStringAnsi(delta.new_file.path));

            if (text.Contains('\n'))
            {
                text = text.Remove(text.IndexOf('\n'));
            }


            if (!string.IsNullOrEmpty(fileName) && eachFileDiff.ContainsKey(fileName))
            {
                eachFileDiff[fileName].Add(text);
            }

            return 0;
        }

        private int DiffLineCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, ref LibGit2Wrapper.GitDiffLine line, IntPtr payload)
        {
            string content = Marshal.PtrToStringAnsi(line.content, (int)line.content_len);
            string? fileName = Path.GetFileName(Marshal.PtrToStringAnsi(delta.new_file.path));
             
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

            if (!string.IsNullOrEmpty(fileName) && eachFileDiff.ContainsKey(fileName))
            {
                eachFileDiff[fileName].Add(text);
            }

            return 0;
        }
    }
}
