using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class TextPart : TextElement
    {
        public Part Part { get; private set; }
        public TextChapter[] Chapters { get; private set; }

        public TextPart(Part Part, IEnumerable<TextChapter> Chapters)
        {
            this.Part = Part;
            this.Chapters = Chapters.ToArray();
            WordCount = this.Chapters.Select(ch => ch.WordCount).Sum();
        }

        public override void ToLatex(LatexWriter writer)
        {
            foreach (var ch in Chapters)
            {
                writer.CurrentChapter = ch;
                ch.ToLatex(writer);
                writer.CurrentChapter = null;
                writer.ForceWriteLine();
            }
        }
    }
}
