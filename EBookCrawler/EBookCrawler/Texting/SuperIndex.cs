using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class SuperIndex : ContainerElement
    {
        public bool IsSub { get; set; }

        public override bool HasExteriorEnvironment()
            => true;

        public override void ToLatex(LatexWriter writer)
        {
            char c = IsSub ? '_' : '^';
            writer.WriteLine(@"${}" + c + @"{\text{");
            base.ToLatex(writer);
            writer.WriteLine(@"}}$");
        }
    }
}
