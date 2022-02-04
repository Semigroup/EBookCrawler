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
                writer.BeginEnvironment("quote");
                writer.Write(@"\glqq{}");
                base.ToLatex(writer);
                writer.Write(@"\grqq{}");
                writer.WriteLine();
                writer.EndEnvironment("quote");
                writer.WriteLine();
            }
            else
            {
                writer.Write(@"\glqq{}");
                base.ToLatex(writer);
                writer.Write(@"\grqq{}");
            }
        }
    }
}
