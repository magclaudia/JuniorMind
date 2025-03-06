using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitClient.model
{
    public class Hunk
    {
        public List<string> FileHeaders { get; } = new List<string>();
        public string? HunkHeader { get; set; } = string.Empty;
        public List<HunkLine> Lines { get; } = new List<HunkLine>();
    }
}
