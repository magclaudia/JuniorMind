using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gitclient.ui
{
    public class SetTextLegth
    {
        public string Text(string text, int width)
        {
            return  text.Length < width ?  text : text.Substring(0, width);
        }
    }
}
