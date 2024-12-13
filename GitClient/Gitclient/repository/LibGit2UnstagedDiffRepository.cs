using Gitclient.model;
using Gitclient.ui;
using GitClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static GitClient.LibGit2Wrapper;

namespace Gitclient.repository
{
    public class LibGit2UnstagedDiffRepository
    {
        private LibGit2Wrapper.GitDiffOptions options = new LibGit2Wrapper.GitDiffOptions();
        private List<Diff> unstagedDiff = new List<Diff>();
        private List<List<string>> diffList = new List<List<string>>();
        private LibGit2UnstagedChangesRepository repository = new LibGit2UnstagedChangesRepository();

        public List<Diff> GetAllUnstagedDiff()
        {
            IntPtr diff = repository.GetDiff();

            if (diff == IntPtr.Zero)
            {
                throw new Exception("Failed to retrieve a valid diff object.");
            }

            nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);
            long testVariable = (long)numDeltas;

            if (numDeltas != 0)
            {
                int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

                if (result != 0)
                {
                    throw new Exception("Failed to iterate over diff.");
                }
            }
            
            foreach (var list in diffList)
            {
                unstagedDiff.Add(new Diff(list));
            }

            return unstagedDiff;
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

            if (diffList.Count > 0)
            {
                if (diffList[diffList.Count - 1].Any(path => path.EndsWith(".cs")))
                {
                    diffList.Add(new List<string>());
                }

                diffList[diffList.Count - 1].Add(text);
            }
            else
            {
                diffList.Add(new List<string>());
                diffList[0].Add(text);
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

            diffList[diffList.Count - 1].Add(text);

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

            diffList[diffList.Count - 1].Add(text.TrimEnd());

            return 0;
        }
    }
}
