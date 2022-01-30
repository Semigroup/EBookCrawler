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

        public override void ToLatex(LatexWriter writer)
        {
            //ToDo

            writer.WriteLine(@"\part{Band " + Part.Reference.Number + "}");
            writer.WriteLineBreak();
            foreach (var ch in Chapters)
            {
                ch.ToLatex(writer);
                writer.WriteLineBreak();
            }
        }
    }
}
