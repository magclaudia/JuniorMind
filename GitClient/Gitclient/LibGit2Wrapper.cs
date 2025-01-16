using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace GitClient
{
    public class LibGit2Wrapper
    {
        public const string libgit2 = "git2";

        static LibGit2Wrapper()
        {
            LoadLibrary();
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct GitOid
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Id;
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
                old_prefix = Marshal.StringToCoTaskMemUTF8("a");
                new_prefix = Marshal.StringToCoTaskMemUTF8("b");
            }
        }


        [Flags]
        public enum SubmoduleIgnore
        {
            GIT_SUBMODULE_IGNORE_UNSPECIFIED = -1,
            GIT_SUBMODULE_IGNORE_NONE = 1,
            GIT_SUBMODULE_IGNORE_UNTRACKED = 2,
            GIT_SUBMODULE_IGNORE_DIRTY = 3,
            GIT_SUBMODULE_IGNORE_ALL = 4
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct GitStrArray
        {
            public IntPtr strings;
            public uint count;
        }


        [Flags]
        public enum GitOidT
        {
            GIT_OID_SHA1 = 1,
        }


        [Flags]
        public enum DiffOptionFlags
        {
            GIT_DIFF_NORMAL = 0,
            GIT_DIFF_REVERSE = 1 << 0,
            GIT_DIFF_INCLUDE_IGNORED = 1 << 1,
            GIT_DIFF_RECURSE_IGNORED_DIRS = 1 << 2,
            GIT_DIFF_INCLUDE_UNTRACKED = 1 << 3,
            GIT_DIFF_RECURSE_UNTRACKED_DIRS = 1 << 4,
            GIT_DIFF_INCLUDE_UNMODIFIED = 1 << 5,
            GIT_DIFF_INCLUDE_TYPECHANGE = 1 << 6,
            GIT_DIFF_INCLUDE_TYPECHANGE_TREES = 1 << 7,
            GIT_DIFF_IGNORE_FILEMODE = 1 << 8,
            GIT_DIFF_IGNORE_SUBMODULES = 1 << 9,
            GIT_DIFF_IGNORE_CASE = 1 << 10,
            GIT_DIFF_INCLUDE_CASECHANGE = 1 << 11,
            GIT_DIFF_DISABLE_PATHSPEC_MATCH = 1 << 12,
            GIT_DIFF_SKIP_BINARY_CHECK = 1 << 13,
            GIT_DIFF_ENABLE_FAST_UNTRACKED_DIRS = 1 << 14,
            GIT_DIFF_UPDATE_INDEX = 1 << 15,
            GIT_DIFF_INCLUDE_UNREADABLE = 1 << 16,
            GIT_DIFF_INCLUDE_UNREADABLE_AS_UNTRACKED = 1 << 17,
            GIT_DIFF_INDENT_HEURISTIC = 1 << 18,
            GIT_DIFF_IGNORE_BLANK_LINES = 1 << 19,
            GIT_DIFF_FORCE_TEXT = 1 << 20,
            GIT_DIFF_FORCE_BINARY = 1 << 21,
            GIT_DIFF_IGNORE_WHITESPACE = 1 << 22,
            GIT_DIFF_IGNORE_WHITESPACE_CHANGE = 1 << 23,
            GIT_DIFF_IGNORE_WHITESPACE_EOL = 1 << 24,
            GIT_DIFF_SHOW_UNTRACKED_CONTENT = 1 << 25,
            GIT_DIFF_SHOW_UNMODIFIED = 1 << 26,
            GIT_DIFF_PATIENCE = 1 << 27,
            GIT_DIFF_MINIMAL = 1 << 28,
            GIT_DIFF_SHOW_BINARY = 1 << 29
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


        [Flags]
        public enum GitDelta
        {
            GIT_DELTA_UNMODIFIED = 0,
            GIT_DELTA_ADDED = 1,
            GIT_DELTA_DELETED = 2,
            GIT_DELTA_MODIFIED = 3,
            GIT_DELTA_RENAMED = 4,
            GIT_DELTA_COPIED = 5,
            GIT_DELTA_IGNORED = 6,
            GIT_DELTA_UNTRACKED = 7,
            GIT_DELTA_TYPECHANGE = 8,
            GIT_DELTA_UNREADABLE = 9,
            GIT_DELTA_CONFLICTED = 10
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


        [StructLayout(LayoutKind.Sequential)]
        public struct GitDiffLine
        {
            public GitDiffLineOrigin origin;
            public int old_lineno;
            public int new_lineno;
            public int num_lines;
            public UIntPtr content_len;
            public long content_offset;
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
            public byte[] header;
        }

        public enum GitObjectType
        {
            GIT_OBJECT_ANY = -2,
            GIT_OBJECT_INVALID = -1,
            GIT_OBJECT_COMMIT = 1,
            GIT_OBJECT_TREE = 2,
            GIT_OBJECT_BLOB = 3,
            GIT_OBJECT_TAG = 4
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffNotifyCallback(IntPtr diff_so_far, GitDiffDelta delta_to_add, IntPtr matched_pathspec, IntPtr payload);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffProgressCallback(IntPtr diff_so_far, IntPtr old_path, IntPtr new_path, IntPtr payload);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffFileCallback(ref GitDiffDelta delta, float progress, IntPtr payload);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffBinaryCallback(ref GitDiffDelta delta, IntPtr binary, IntPtr payload);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffHunkCallback(ref GitDiffDelta delta, ref GitDiffHunk hunk, IntPtr payload);


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DiffLineCallback(ref GitDiffDelta delta, ref GitDiffHunk hunk, ref GitDiffLine line, IntPtr payload);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_diff_foreach(IntPtr diff, DiffFileCallback fileCallback, DiffBinaryCallback binaryCallback, DiffHunkCallback hunkCallback,
            DiffLineCallback lineCallback, IntPtr payload);


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


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_diff_index_to_workdir(out IntPtr diff, IntPtr repo, IntPtr index, ref GitDiffOptions options);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_reference_name_to_id(out GitOid id, IntPtr repo, string name);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_repository_index(out IntPtr index, IntPtr repo);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_diff_tree_to_index(out IntPtr diff, IntPtr repo,  IntPtr tree, IntPtr index, ref GitDiffOptions options);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_index_free(IntPtr index);


        //[DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int git_index_add_all(IntPtr index, ref GitStrArray pathspec, uint flags, IntPtr callback, IntPtr payload);

        //[DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int git_index_write(IntPtr index);

        //[DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int git_diff_tree_to_workdir(out IntPtr diff, IntPtr repo, IntPtr tree, ref GitDiffOptions options);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_diff_tree_to_workdir_with_index(out IntPtr diff, IntPtr repo, IntPtr tree, ref GitDiffOptions options);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_repository_head(out IntPtr tree, IntPtr repo);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_index_read(IntPtr index, int force);

        

        
        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_reference_peel(out IntPtr peeledObject, IntPtr reference, GitObjectType targetType);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_reference_free(IntPtr reference);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_index_add_bypath(IntPtr index, string path);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_index_remove_bypath(IntPtr index, string path);


        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_index_write(IntPtr index);



        //[DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int git_index_remove(IntPtr index, string path, int stage);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_reset_default(IntPtr repo, IntPtr target, ref GitStrArray pathspec);



        public static void LoadLibrary()
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

    }
}
