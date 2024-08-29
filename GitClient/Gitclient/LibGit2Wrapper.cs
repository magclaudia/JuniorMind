using System.Runtime.InteropServices;

namespace GitClient
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GitOid
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Id;
    }

    public class LibGit2Wrapper
    {
        private const string libgit2 = "git2";

        static LibGit2Wrapper()
        {
            LoadLibrary();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GitDiffOptions
        {
            public uint version;
            public uint flags;
            public SubmoduleIgnore ignoreSubmodules;
            public GitStrArray pathspec;
            public DiffNotifyCallback? notify_cb;
            public DiffProgressCallback? progress_cb;
            public IntPtr payload;
            public uint context_lines;
            public uint interhunk_lines;
            public GitOidT oid_type;
            public ushort id_abbrev;
            public long max_size;
            public IntPtr old_prefix;
            public IntPtr new_prefix;

            public GitDiffOptions()
            {
                version = 1;
                flags = (uint)DiffOptionFlags.GIT_DIFF_NORMAL;
                ignoreSubmodules = SubmoduleIgnore.GIT_SUBMODULE_IGNORE_NONE;
                pathspec = new GitStrArray { strings = IntPtr.Zero, count = 0 };
                notify_cb = null;
                progress_cb = null;
                payload = IntPtr.Zero;
                context_lines = 3;
                interhunk_lines = 0;
                oid_type = GitOidT.GIT_OID_SHA1;
                id_abbrev = 7;
                max_size = 512 * 1024 * 1024;
                old_prefix = Marshal.StringToHGlobalAnsi("a");
                new_prefix = Marshal.StringToHGlobalAnsi("b");
            }
        }

        public enum SubmoduleIgnore
        {
            GIT_SUBMODULE_IGNORE_UNSPECIFIED,
            GIT_SUBMODULE_IGNORE_NONE,
            GIT_SUBMODULE_IGNORE_UNTRACKED,
            GIT_SUBMODULE_IGNORE_DIRTY,
            GIT_SUBMODULE_IGNORE_ALL
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GitStrArray
        {
            public IntPtr strings;
            public uint count;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffNotifyCallback(IntPtr diff_so_far, IntPtr delta_to_add, IntPtr matched_pathspec, IntPtr payload);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffProgressCallback(IntPtr diff_so_far, IntPtr old_path, IntPtr new_path, IntPtr payload);

        public enum GitOidT
        {
            GIT_OID_SHA1
        }

        [Flags]
        public enum DiffOptionFlags
        {
            GIT_DIFF_NORMAL = 0
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GitDiffDelta
        {
            public GitDelta status;
            public GitDiffFlags flags;
            public ushort similarity;
            public ushort nfiles;
            public GitDiffFile old_file;
            public GitDiffFile new_file;
        }

        public enum GitDelta
        {
            GIT_DELTA_UNMODIFIED,
            GIT_DELTA_ADDED,
            GIT_DELTA_DELETED,
            GIT_DELTA_MODIFIED,
            GIT_DELTA_RENAMED,
            GIT_DELTA_COPIED,
            GIT_DELTA_IGNORED,
            GIT_DELTA_UNTRACKED,
            GIT_DELTA_TYPECHANGE,
            GIT_DELTA_UNREADABLE,
            GIT_DELTA_CONFLICTED
        }

        [Flags]
        public enum GitDiffFlags
        {
            GIT_DIFF_FLAG_BINARY,
            GIT_DIFF_FLAG_NOT_BINARY,
            GIT_DIFF_FLAG_VALID_ID,
            GIT_DIFF_FLAG_EXISTS,
            GIT_DIFF_FLAG_VALID_SIZE
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GitDiffFile
        {
            public GitOid id;
            public IntPtr path;
            public long size;
            public GitDiffFlags flags;
            public ushort mode;
            public ushort id_abbrev;
        }

        private static void LoadLibrary()
        {
            string libName;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                libName = $"{libgit2}.dll";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                libName = $"lib{libgit2}.so";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                libName = $"lib{libgit2}.dylib";
            }
            else
            {
                throw new PlatformNotSupportedException("Platform not supported.");
            }

            string libPath = Path.Combine(AppContext.BaseDirectory, libName);
            IntPtr libHandle = NativeLibrary.Load(libPath);
            if (libHandle == IntPtr.Zero)
            {
                throw new FileNotFoundException($"Failed to load {libName} from {libPath}.");
            }
        }

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_libgit2_init();

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_repository_open(out IntPtr repo, string path);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_repository_free(IntPtr repo);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_revwalk_new(out IntPtr walker, IntPtr repo);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_revwalk_push_head(IntPtr walker);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_revwalk_next(out GitOid id, IntPtr walker);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_commit_lookup(out IntPtr commit, IntPtr repo, ref GitOid id);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr git_commit_author(IntPtr commit);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern long git_commit_time(IntPtr commit);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr git_commit_message(IntPtr commit);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_commit_free(IntPtr commit);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_revwalk_free(IntPtr walker);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint git_commit_parentcount(IntPtr commit);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_commit_parent(out IntPtr parent, IntPtr commit, uint n);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_commit_tree(out IntPtr treeOut, IntPtr commit);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_tree_free(IntPtr tree);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_diff_tree_to_tree(out IntPtr diff, IntPtr repo, IntPtr oldTree, IntPtr newTree, ref GitDiffOptions options);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern UIntPtr git_diff_num_deltas(IntPtr diff);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr git_diff_get_delta(IntPtr diff, UIntPtr id);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_diff_free(IntPtr diff);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_blob_lookup(out IntPtr blob, IntPtr repo, ref GitOid id);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr git_blob_rawcontent(IntPtr blob);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern long git_blob_rawsize(IntPtr blob);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_blob_free(IntPtr blob);



        [StructLayout(LayoutKind.Sequential)]
        public struct GitDiffLine
        {
            public GitDiffLineOrigin origin;
            public int old_lineno;
            public int new_lineno;
            public int num_lines;
            public long content_len;
            public IntPtr content_offset;
            public IntPtr content;
        }

        public enum GitDiffLineOrigin : byte
        {
            GIT_DIFF_LINE_CONTEXT = 0x20, //' ',
            GIT_DIFF_LINE_ADDITION = 0x2B, //'+',
            GIT_DIFF_LINE_DELETION = 0x2D, //'-',
            GIT_DIFF_LINE_CONTEXT_EOFNL = 0x3D, //'=',
            GIT_DIFF_LINE_ADD_EOFNL = 0x3E, //'>',
            GIT_DIFF_LINE_DEL_EOFNL = 0x3C, //'<',
            GIT_DIFF_LINE_FILE_HDR = 0x46, //'F',
            GIT_DIFF_LINE_HUNK_HDR = 0x48, //'H',
            GIT_DIFF_LINE_BINARY = 0x42, //'B'
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GitDiffHunk
        {
            public int old_start;
            public int old_lines;
            public int new_start;
            public int new_lines;
            public UIntPtr header_len;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] header;
        }

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_diff_foreach(IntPtr diff, DiffFileCallback fileCallback, DiffBinaryCallback binaryCallback, DiffHunkCallback hunkCallback,
            DiffLineCallback lineCallback, IntPtr payload);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffFileCallback(GitDiffDelta delta, float progress, IntPtr payload);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffBinaryCallback(GitDiffDelta delta, IntPtr binary, IntPtr payload);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffHunkCallback(GitDiffDelta delta, GitDiffHunk hunk, IntPtr payload);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffLineCallback(GitDiffDelta delta, GitDiffHunk hunk, GitDiffLine line, IntPtr payload);
    }
}
