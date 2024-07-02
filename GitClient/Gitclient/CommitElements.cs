using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitClient
{
    public class CommitElements
    {
        public List<GitOid> IdGitOid;
        public List<string> Id;
        public List<string> DateTime;
        public List<string> Author;
        public List<string> Message;
        public CommitElements()
        {
            IdGitOid = new List<GitOid>();
            Id = new List<string>();
            DateTime = new List<string>();
            Author = new List<string>();
            Message = new List<string>();
        }
    }
}
