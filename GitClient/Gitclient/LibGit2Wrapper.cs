using System;
using System.Runtime.InteropServices;


namespace GitClient
{
    [StructLayout(LayoutKind.Sequential)]
    public struct GitOid
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Id;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GitDiffDelta
    {
        public GitDelta status;
        public GitDiffFile old_file;
        public GitDiffFile new_file;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GitDiffFile
    {
        public IntPtr path;
        public ulong size;
        public uint flags;
        public uint mode;
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


    public class LibGit2Wrapper
    {
        private const string libgit2 = "git2";

        static LibGit2Wrapper()
        {
            LoadLibrary();
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
        public static extern int git_diff_tree_to_tree(out IntPtr diff, IntPtr repo, IntPtr oldTree, IntPtr newTree, IntPtr opts);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_diff_num_deltas(IntPtr diff);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr git_diff_get_delta(IntPtr diff, int id);

        [DllImport(libgit2, CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_diff_free(IntPtr diff);

    }
}
