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
        public Alignment MyAlignment { get; set; } = Alignment.Unspecified;

        public override void ToLatex(LatexWriter writer)
        {
            string left = Length.GetLeftSpace(MyAlignment);
            string right = Length.GetRightSpace(MyAlignment);

            writer.EndLine();
            writer.Write(@"{");

            writer.Write(@"\rule{0.0\textwidth}{0.4pt}");
            writer.Write(@"\hspace{" + left + "}");
            writer.Write(@"\rule{" + Length+ "}{0.4pt}");
            writer.Write(@"\hspace{" + right + "}");

            writer.WriteLine(@"}");
        }
    }
}
