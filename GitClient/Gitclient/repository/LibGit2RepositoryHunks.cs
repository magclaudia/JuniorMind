using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitClient.ui;
using GitClient.service;
using System.Runtime.InteropServices;
using LibGit2Sharp;
using System.Reflection;
using static GitClient.LibGit2Wrapper;


namespace GitClient.repository
{

    public class LibGit2RepositoryHunks
    {
        private LibGit2RepositoryChanges libGit2RepositoryChanges;
        private LibGit2Wrapper.GitDiffOptions options;
        private int index;

        public LibGit2RepositoryHunks(LibGit2RepositoryChanges libGit2RepositoryChanges)
        {
            this.libGit2RepositoryChanges = libGit2RepositoryChanges;
            options = new LibGit2Wrapper.GitDiffOptions();
        }

        public void StageOrUnstageHunk(string filePath, int hunkIndex, List<string> hunk, int fileIndex)
        {
            index = hunkIndex;
            IntPtr repo = libGit2RepositoryChanges.GetRepo();
            IntPtr diff = libGit2RepositoryChanges.GetDiff();
            IntPtr patch = IntPtr.Zero;

            if (LibGit2Wrapper.git_patch_from_diff(out patch, diff, (UIntPtr)hunkIndex) != 0)
            {

            }

            LibGit2Wrapper.GitDiffHunk hunkPtr;
            UIntPtr linesInHunk;

            if (LibGit2Wrapper.git_patch_get_hunk(out hunkPtr, out linesInHunk, patch, (UIntPtr)fileIndex) != 0)
            {

            }


            const int GIT_APPLY_LOCATION_INDEX = 0;
            const int GIT_APPLY_LOCATION_WORKDIR = 1;

            int applyLocation = ReadButtons.WorkingInStagePanel == true ? GIT_APPLY_LOCATION_INDEX : GIT_APPLY_LOCATION_WORKDIR;
            int applyResult = LibGit2Wrapper.git_apply(repo, diff, applyLocation, IntPtr.Zero);
            
            if (applyResult != 0)
            {
                throw new Exception($"Failed to apply hunk at location {applyLocation}: {applyResult}");
            }
        }

        //private IntPtr GetPatchForFile(IntPtr diff, string filePath)
        //{
        //    IntPtr patch = IntPtr.Zero;

        //    UIntPtr numberOfDeltas = LibGit2Wrapper.git_diff_num_deltas(diff);

        //    for (UIntPtr i = UIntPtr.Zero; i.ToUInt64() < numberOfDeltas.ToUInt64(); i = new UIntPtr(i.ToUInt64() + 1))
        //    {
        //        IntPtr delta = LibGit2Wrapper.git_diff_get_delta(diff, i);

        //        if (delta == IntPtr.Zero)
        //        {
        //            throw new Exception($"Failed to retrieve delta at index {i}.");
        //        }

        //        LibGit2Wrapper.GitDiffDelta deltaStruct = Marshal.PtrToStructure<LibGit2Wrapper.GitDiffDelta>(delta);
        //        string deltaFilePath = Marshal.PtrToStringAnsi(deltaStruct.new_file.path)!;

        //        if (string.Equals(deltaFilePath, filePath, StringComparison.OrdinalIgnoreCase))
        //        {
        //            int result = LibGit2Wrapper.git_patch_from_diff(out patch, diff, i);

        //            if (result != 0 || patch == IntPtr.Zero)
        //            {
        //                throw new Exception($"Failed to create patch for file '{filePath}': {result}");
        //            }

        //            return patch;
        //        }
        //    }

        //    throw new FileNotFoundException($"File '{filePath}' not found in diff.");
        //}

        //public void StageOrUnstageHunk(IntPtr repo, IntPtr diff, string filePath, int hunkIndex)
        //{
        //    IntPtr patch = GetPatchForFile(diff, filePath);

        //    try
        //    {
        //        ApplyHunk(repo, diff, patch, hunkIndex);
        //    }
        //    finally
        //    {
        //        if (patch != IntPtr.Zero)
        //        {
        //           // LibGit2Wrapper.git_patch_free(patch); // Free the patch
        //        }
        //    }
        //}

        //private LibGit2Wrapper.GitDiffHunk GetHunkFromPatch(IntPtr patch, int hunkIndex)
        //{
        //    LibGit2Wrapper.GitDiffHunk hunkPtr;
        //    UIntPtr lineCount = UIntPtr.Zero;

        //    int result = LibGit2Wrapper.git_patch_get_hunk(out hunkPtr, out lineCount, patch, (UIntPtr)hunkIndex);

        //    if (result != 0)
        //    {
        //        throw new Exception($"Failed to retrieve hunk at index {hunkIndex}: {result}");
        //    }

        //    return hunkPtr;
        //}

        //private void ApplyHunk(IntPtr repo, IntPtr diff, IntPtr patch, int hunkIndex)
        //{
        //    LibGit2Wrapper.GitDiffHunk hunk = GetHunkFromPatch(patch, hunkIndex);

        //    //if (hunk == IntPtr.Zero)
        //    //{
        //    //    throw new Exception($"Hunk at index {hunkIndex} could not be retrieved.");
        //    //}

        //    const int GIT_APPLY_LOCATION_INDEX = 0;
        //    const int GIT_APPLY_LOCATION_WORKDIR = 1;

        //    int applyLocation = ButtomPress.Type.workingInStagePanel == true ? GIT_APPLY_LOCATION_INDEX : GIT_APPLY_LOCATION_WORKDIR;
        //    int result = LibGit2Wrapper.git_apply(repo, patch, applyLocation, IntPtr.Zero);

        //    if (result != 0)
        //    {
        //        throw new Exception($"Failed to apply hunk at index {hunkIndex}: {result}");
        //    }
        //}

        //private void StageHunk(IntPtr diff, IntPtr repo, UIntPtr hunkIndex, List<string> hunk)
        //{
        //    IntPtr patch = IntPtr.Zero;

        //    try
        //    {
        //        int result = LibGit2Wrapper.git_patch_from_diff(out patch, diff, hunkIndex);

        //        if (result != 0)
        //        {
        //            throw new Exception($"Failed to create patch for hunk: {result}");
        //        }

        //        const int GIT_APPLY_LOCATION_INDEX = 0;
        //        result = LibGit2Wrapper.git_apply(repo, patch, GIT_APPLY_LOCATION_INDEX, IntPtr.Zero);

        //        if (result != 0)
        //        {
        //            throw new Exception($"Failed to apply patch to index: {result}");
        //        }
        //    }
        //    finally
        //    {
        //        if (patch != IntPtr.Zero)
        //        {
        //            // LibGit2Wrapper.git_patch_free(patch); 
        //        }
        //    }
        //}

        //private void UnstageHunk(IntPtr diff, IntPtr repo, UIntPtr hunkIndex, List<string> hunk)
        //{
        //    IntPtr patch = IntPtr.Zero;

        //    try
        //    {
        //        int result = LibGit2Wrapper.git_patch_from_diff(out patch, diff, hunkIndex);

        //        if (result != 0)
        //        {
        //            throw new Exception($"Failed to create patch for hunk: {result}");
        //        }

        //        const int GIT_APPLY_LOCATION_WORKDIR = 1;
        //        result = LibGit2Wrapper.git_apply(repo, patch, GIT_APPLY_LOCATION_WORKDIR, IntPtr.Zero);

        //        if (result != 0)
        //        {
        //            throw new Exception($"Failed to unstage patch: {result}");
        //        }
        //    }
        //    finally
        //    {
        //        if (patch != IntPtr.Zero)
        //        {
        //            // LibGit2Wrapper.git_patch_free(patch); // Ensure you free the patch after use if needed
        //        }
        //    }
        //}
    }
}
