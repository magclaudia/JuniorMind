using GitClient.model;
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
    public class UnstageLibGit2DiffRepository
    {
        private LibGit2Repository libGit2Repository;
        private LibGit2Wrapper.GitDiffOptions options;
        private List<FileDiff> unstagedDiff;
        private Dictionary<string, List<string>> eachFileDiff;

        public UnstageLibGit2DiffRepository(LibGit2Repository libGit2Repository)
        {
            this.libGit2Repository = libGit2Repository;
            options = new LibGit2Wrapper.GitDiffOptions();
            unstagedDiff = new List<FileDiff>();
            eachFileDiff = new Dictionary<string, List<string>>();
        }

        public List<FileDiff> GetAllUnstagedDiff(IntPtr diff)
        {
            if (diff == IntPtr.Zero)
            {
                throw new Exception("Failed to retrieve a valid diff object.");
            }

            nuint numDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);
            long testVariable = (long)numDeltas;
            
            unstagedDiff.Clear();

            if (numDeltas != 0)
            {
                int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);

                if (result != 0)
                {
                    throw new Exception("Failed to iterate over diff.");
                }
            }

            foreach (var entry in eachFileDiff)
            {
                unstagedDiff.Add(new FileDiff(entry.Value, entry.Key));
            }

            return unstagedDiff;
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


            if (!string.IsNullOrEmpty(filePath) && !eachFileDiff.ContainsKey(filePath))
            {
                eachFileDiff[filePath] = [text];
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


            if (!string.IsNullOrEmpty(filePath) && eachFileDiff.ContainsKey(filePath))
            {
                eachFileDiff[filePath].Add(text);
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

            if (!string.IsNullOrEmpty(filePath) && eachFileDiff.ContainsKey(filePath))
            {
                eachFileDiff[filePath].Add(text);
            }

            return 0;
        }
    }
}
