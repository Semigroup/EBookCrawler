using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Document : TextElement
    {
        public BookReference Book { get; private set; }
        public TextPart[] Parts { get; private set; }

        public Document(BookReference Book, IEnumerable<TextPart> Parts)
        {
            this.Book = Book;
            this.Parts = Parts.ToArray();
            this.WordCount = Parts.Select(p => p.WordCount).Sum();
        }

        public override void ToLatex(LatexWriter writer)
        {
            //ToDo: Title, Toc

            writer.WritePreamble();
            writer.WriteLine(@"\begin{document}");

            foreach (var part in Parts)
            {
                writer.WriteLineBreak(2);
                if (Parts.Length > 1)
                {
                    writer.WriteLine(@"\part{ " + part.Part.Reference.Name + "}");
                    writer.WriteLineBreak();
                }
                part.ToLatex(writer);
            }

            writer.WriteLine(@"\end{document}");
        }
    }
}
