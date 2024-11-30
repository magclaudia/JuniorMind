using Gitclient.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gitclient.repository

{
    public class LibGit2Repository
    {
        public List<UnstagedChange> getAllUnstagedChanges()
        {
            // call API
            return new List<UnstagedChange>()
            {
            new UnstagedChange(ChangeType.ADDED, "my file name.cs", "my path")
            };
        }

    }
}
