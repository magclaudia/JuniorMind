using GitClient.model;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static GitClient.LibGit2Wrapper;

namespace GitClient.repository
{
    public class StageLibGit2DiffRepository
    {
        private LibGit2Repository libGit2Repository;
        private LibGit2Wrapper.GitDiffOptions options;
        private List<Diffs> stageDiffs;
        private List<List<string>> stageDiffsList;

        public StageLibGit2DiffRepository(LibGit2Repository libGit2Repository)
        {
            this.libGit2Repository = libGit2Repository;
            options = new LibGit2Wrapper.GitDiffOptions();
            stageDiffs = new List<Diffs>();
            stageDiffsList = new List<List<string>>();
        }

        public List<Diffs> GetAllStageDiffs()
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

            foreach(var eachStageDiff in stageDiffsList)
            {
                stageDiffs.Add(new Diffs(eachStageDiff));
            }

            return stageDiffs;
        }

        private int DiffFileCallback(ref LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringAnsi(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringAnsi(delta.new_file.path);
            string text = "";

            if (string.IsNullOrEmpty(oldFilePath) && string.IsNullOrEmpty(newFilePath))
            {
                throw new Exception("Paths are null or empty.");
            }

            if (!string.IsNullOrEmpty(newFilePath))
            {
                text = newFilePath;
            }

            if (stageDiffsList.Count > 0)
            {
                if (stageDiffsList[stageDiffsList.Count - 1].Any(path => path.EndsWith(".cs")))
                {
                    stageDiffsList.Add(new List<string>());
                }

                stageDiffsList[stageDiffsList.Count - 1].Add(text);
            }
            else
            {
                stageDiffsList.Add(new List<string>());
                stageDiffsList[0].Add(text);
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

            if (text.Contains('\n'))
            {
                text = text.Remove(text.IndexOf('\n'));
            }

            stageDiffsList[stageDiffsList.Count - 1].Add(text);

            return 0;
        }

        private int DiffLineCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, ref LibGit2Wrapper.GitDiffLine line, IntPtr payload)
        {
            string content = Marshal.PtrToStringAnsi(line.content, (int)line.content_len);

            if (content.StartsWith('\t'))
            {
                string output = content.Replace("\t", new string(' ', 4));
                content = output + content;
            }

            if (content.Contains("\n\t"))
            {
                content = content[..content.IndexOf("\n\t")];
            }

            string text = $"{(char)line.origin} {content}";

            stageDiffsList[stageDiffsList.Count - 1].Add(text.TrimEnd());

            return 0;
        }
    }
}
