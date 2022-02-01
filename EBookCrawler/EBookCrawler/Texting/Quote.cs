using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Quote : ContainerElement
    {
        public bool IsBlock { get; set; }

        public override bool HasExteriorEnvironment()
            => IsBlock;

        public override void ToLatex(LatexWriter writer)
        {
            if (IsBlock)
            {
                writer.WriteLine(@"\begin{quote}");
                writer.WriteLine(@"\glqq");
                base.ToLatex(writer);
                writer.WriteLine(@"\grqq");
                writer.WriteLine(@"\end{quote}");
            }
            else
            {
                writer.WriteLine(@"\glqq");
                base.ToLatex(writer);
                writer.WriteLine(@"\grqq");
            }
        }
    }
}
