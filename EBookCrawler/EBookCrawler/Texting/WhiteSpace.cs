using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
  public  class WhiteSpace : TextElement
    {
        public double HSpace { get; set; }
        public double VSpace { get; set; }
        public bool NonBreakingSpace { get; set; }
        public double Indentation { get; set; }

        public override void ToLatex(LatexWriter writer, LatexContext context)
        {

        }
    }
}
