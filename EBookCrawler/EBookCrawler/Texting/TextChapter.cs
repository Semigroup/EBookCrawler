using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class TextChapter : ContainerElement
    {
        public Chapter Chapter { get; set; }

        public override void ToLatex(LatexWriter writer)
        {
            //ToDo
            writer.WriteLine(@"\chapter{Kapitel " + Chapter.Number + "}");

            base.ToLatex(writer);
        }
    }
}
