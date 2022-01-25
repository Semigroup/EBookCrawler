using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Image : TextElement
    {
        public string Title { get; set; }
        public string AlternativeTitle { get; set; }
        public string RelativePath { get; set; }
        public string Caption { get; set; }

        public Length Height { get; set; }
        public Length Width { get; set; }
        public double VSpace { get; set; }
        public double HSpace { get; set; }
        public double Border { get; set; }

        public bool InLine { get; set; }
        public int Alignment { get; set; }
    }
}
