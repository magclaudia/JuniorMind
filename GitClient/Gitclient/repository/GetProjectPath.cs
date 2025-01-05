using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.repository
{
    public class GetProjectPath
    {
        public string ProjectPath(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                return string.Empty;
            }

            if (Directory.Exists(Path.Combine(directory, ".git")))
            {
                return directory;
            }

            var parentDirectory = Directory.GetParent(directory)?.FullName;
            
            if (string.IsNullOrEmpty(parentDirectory))
            {
                return string.Empty;
            }

            return ProjectPath(parentDirectory);
        }
    }
}
