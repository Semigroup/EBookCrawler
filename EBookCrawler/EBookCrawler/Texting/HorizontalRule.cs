using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class HorizontalRule : TextElement
    {
        public Length Length = new Length() { Value = 1, IsProportional = true };
        public int Alignment { get; set; }

        public override void ToLatex(LatexWriter writer)
        {
            string left = Length.GetLeftSpace(Alignment);
            string right = Length.GetRightSpace(Alignment);

            writer.WriteLine(@"\\");
            writer.WriteLine(@"{");

            writer.Write(@"\rule{0.0\textwidth}{0.4pt}");
            writer.Write(@"\hspace{" + left + "}");
            writer.Write(@"\rule{" + Length+ "}{4pt}");
            writer.WriteLine(@"\hspace{" + right + "}");

            writer.WriteLine(@"}");
        }
    }
}
