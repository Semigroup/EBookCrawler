using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Poem : ContainerElement
    {
        public override void ToLatex(LatexWriter writer)
        {
            writer.WriteLine(@"\begin{quote}");
            base.ToLatex(writer);
            writer.WriteLine(@"\end{quote}");
        }
    }
}
