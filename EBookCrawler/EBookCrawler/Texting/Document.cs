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
            SetTitlePage(writer);
            writer.WriteLine(@"\begin{document}");
            writer.WriteLine(@"\maketitle");

            writer.WriteLine("Extrahiert und kompiliert vom Gutenberg-Projekt am " + DateTime.Now + @".");
            writer.WriteLine(@"\begin{itemize}");
            foreach (var part in Parts)
            {
                var refe = part.Part.Reference;
                writer.Write(@"\item ");
                writer.Write(@"\href{");
                writer.Write(refe.Link);
                writer.Write(@"}{");
                writer.Write(refe.Name);
                if (refe.SubTitle != null && refe.SubTitle.Length > 0)
                {
                    writer.Write(", ");
                    writer.Write(refe.SubTitle);
                }
                writer.Write(@"}");
                writer.ForceWriteLine(1);
            }
            writer.WriteLine(@"\end{itemize}");
            writer.ForceWriteLine(2);
            writer.WriteLine(@"\tableofcontents");

            foreach (var part in Parts)
            {
                writer.ForceWriteLine(2);
                if (Parts.Length > 1)
                {
                    writer.WriteLine(@"\part{ " + part.Part.Reference.Name + "}");
                    writer.ForceWriteLine();
                }
                part.ToLatex(writer);
            }

            writer.WriteLine(@"\end{document}");
        }
        protected void SetTitlePage(LatexWriter writer)
        {
            var meta = Parts[0].Chapters[0].Chapter.MyMeta;

            writer.Write(@"\author{");
            writer.Write(meta.Author);
            if (meta.Translator != null && meta.Translator.Length > 0)
                writer.Write(@"\\{\normalsize Übersetzt von " + meta.Translator + @"}");
            writer.WriteLine("}");

            writer.Write(@"\title{");
            writer.Write(Book.Name);
            if (Book.SubTitle != null && Book.SubTitle.Length > 0 && !Book.PartsHaveDifferentSubTitles)
                writer.Write(@"\\{\large " + Book.SubTitle + @"}");
            writer.WriteLine("}");

            writer.Write(@"\date{" + meta.Year + "}");
        }
    }
}
