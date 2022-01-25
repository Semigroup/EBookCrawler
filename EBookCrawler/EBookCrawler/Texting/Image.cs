using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Image : TextElement
    {
        public string Name { get; set; }
        public string RelativePath { get; set; }
        public string Caption { get; set; }

        public double Height { get; set; }
        public double Width { get; set; }
        public double VSpace { get; set; }
        public double HSpace { get; set; }
    }
}
