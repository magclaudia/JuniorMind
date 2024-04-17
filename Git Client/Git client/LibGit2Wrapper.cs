using System;
using System.Runtime.InteropServices;


namespace GitClient
{
    public struct GitOid
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] Id;
    }

    public class LibGit2Wrapper
    {
        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_libgit2_init();

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_repository_open(out IntPtr repo, string path);
        
        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_repository_free(IntPtr repo);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_revwalk_new(out IntPtr walker, IntPtr repo);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_revwalk_push_head(IntPtr walker);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_revwalk_next(out GitOid id, IntPtr walker);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int git_commit_lookup(out IntPtr commit, IntPtr repo, GitOid id);
        
        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr git_commit_author(IntPtr commit);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern long git_commit_time(IntPtr commit);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr git_commit_message(IntPtr commit);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_commit_free(IntPtr commit);

        [DllImport("git2.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void git_revwalk_free(IntPtr walker);
    }
}
