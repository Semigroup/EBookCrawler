using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Document : TextElement
    {
        public BookReference Book { get; set; }
        public TextPart[] Parts { get; set; }

        public override void ToLatex(LatexWriter writer)
        {
            //ToDo: Title, Toc

            writer.WritePreamble();
            writer.WriteLine(@"\begin{document}");

            foreach (var part in Parts)
            {
                writer.WriteLineBreak(2);
                part.ToLatex(writer);
            }

            writer.WriteLine(@"\end{document}");
        }
    }
}
