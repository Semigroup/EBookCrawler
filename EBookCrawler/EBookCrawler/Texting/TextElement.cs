using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public abstract class TextElement
    {
        public bool IsVisible { get; set; } = true;
        //public string Class { get; set; }
        public abstract void ToLatex(LatexWriter writer);
    }
}
