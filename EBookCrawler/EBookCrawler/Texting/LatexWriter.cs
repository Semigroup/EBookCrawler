using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EBookCrawler.Texting
{
    public class LatexWriter : StreamWriter
    {
        public LatexWriter(string path) : base(path)
        {

        }

        public void WritePreamble()
        {

        }
    }
}
