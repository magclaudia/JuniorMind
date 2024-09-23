using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Testing_git_diff_foreach_API
{
    public class GetCommit
    {
        public static IntPtr GetFirstCommit(IntPtr repo, Get_CommitElement.Elements element)
        {
            int count = 0;
            IntPtr commitPtr = IntPtr.Zero;
            IntPtr walker = IntPtr.Zero;
            LibGit2Wrapper.GitOid id = new LibGit2Wrapper.GitOid();

            if (LibGit2Wrapper.git_revwalk_new(out walker, repo) == 0)
            {
                if (LibGit2Wrapper.git_revwalk_push_head(walker) == 0)
                {
                    while (LibGit2Wrapper.git_revwalk_next(out id, walker) == 0)
                    {
                        if (LibGit2Wrapper.git_commit_lookup(out commitPtr, repo, ref id) == 0)
                        {
                            if (count == 1)
                            {
                                Console.Write($"{element.Id} {element.DateTime} {element.Author}       {element.Message}");
                                break;
                            }

                            element.IdGitOid = id;
                            element.Id = Get_CommitElement.GetCommitId(id);
                            element.DateTime = Get_CommitElement.GetDateAndTime(commitPtr);
                            element.Author = Get_CommitElement.GetCommitAuthor(commitPtr);
                            element.Message = Marshal.PtrToStringUTF8(LibGit2Wrapper.git_commit_message(commitPtr))!;
                            count++;
                        }
                    }
                }
            }

            LibGit2Wrapper.git_revwalk_free(walker);
            return commitPtr;
        }
    }
}
