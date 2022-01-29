using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
   public class EmptyContainer : ContainerElement
    {
        public override void Add(IEnumerable<TextElement> textElements)
        {
        }
        public override void Add(TextElement textElement)
        {
        }
        public override void SetClass(string classValue)
        {
        }
        public override void ToLatex(LatexWriter writer, LatexContext context)
        {
        }
    }
}
