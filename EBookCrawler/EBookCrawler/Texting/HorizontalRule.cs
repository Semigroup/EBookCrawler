using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class HorizontalRule : TextElement
    {
        public Length Length = new Length() { Value = 1, IsProportional = true };
        public int Alignment { get; set; }
    }
}
