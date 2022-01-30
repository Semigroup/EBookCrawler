using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Verbatim : TextElement
    {
        public string Text { get; set; }

        public override void ToLatex(LatexWriter writer)
        {
            // Text may not contain "\end{verbatim}"
            if (Text.Contains(@"\end{verbatim}"))
                throw new NotImplementedException();
            writer.WriteLine(@"\begin{verbatim}");
            writer.WriteLine(Text);
            writer.WriteLine(@"\end{verbatim}");
        }
    }
}
