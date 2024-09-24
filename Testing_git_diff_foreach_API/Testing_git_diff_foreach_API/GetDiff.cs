using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Testing_git_diff_foreach_API
{
    public class GetDiff
    {
        public static void GetFirstDiff(IntPtr diff)
        {
            int result = LibGit2Wrapper.git_diff_foreach(diff, DiffFileCallback, DiffBinaryCallback, DiffHunkCallback, DiffLineCallback, IntPtr.Zero);
            if (result != 0)
            {
                throw new Exception("Failed to iterate over diff.");
            }
        }

        public static int DiffFileCallback(ref LibGit2Wrapper.GitDiffDelta delta, float progress, IntPtr payload)
        {
            string? oldFilePath = Marshal.PtrToStringUTF8(delta.old_file.path);
            string? newFilePath = Marshal.PtrToStringUTF8(delta.new_file.path);
            Console.Write(newFilePath);
            Console.WriteLine();
            return 0;
        }

        public static int DiffBinaryCallback(ref LibGit2Wrapper.GitDiffDelta delta, IntPtr binary, IntPtr payload)
        {
            return 0;
        }

        public static int DiffHunkCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, IntPtr payload)
        {
            byte[] filteredHeader = hunk.header.Where(c => c != '\0' && c != '0').ToArray();
            string hunkHeader = System.Text.Encoding.UTF8.GetString(filteredHeader);
            Console.Write(hunkHeader);
            Console.WriteLine();
            return 0;
        }

        public static int DiffLineCallback(ref LibGit2Wrapper.GitDiffDelta delta, ref LibGit2Wrapper.GitDiffHunk hunk, ref LibGit2Wrapper.GitDiffLine line, IntPtr payload)
        {
            string content = Marshal.PtrToStringUTF8(line.content)!;
            Console.Write(content);
            Console.WriteLine();
            return 0;
        }
    }
}
