using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Box : ContainerElement
    {
        public override bool HasExteriorEnvironment()
             => true;

        public override void ToLatex(LatexWriter writer)
        {
            //ToDo: add Box in Latex
            base.ToLatex(writer);
        }
    }
}
