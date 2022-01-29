using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class TextPart : TextElement
    {
        public Part Part { get; set; }
        public TextChapter[] Chapters { get; set; }
    }
}
