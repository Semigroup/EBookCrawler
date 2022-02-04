using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class WhiteSpace : TextElement
    {
        public double HSpace { get; set; }
        public double VSpace { get; set; }
        public bool NonBreakingSpace { get; set; }
        public double Indentation { get; set; }

        public override void ToLatex(LatexWriter writer)
        {
            if (VSpace > 0)
            {
                if (Indentation > 0)
                {
                    if (VSpace == 1)
                        if (writer.LineIsEmpty)
                            writer.ForceWriteLine();
                        else
                            writer.ForceWriteLine(2);
                    else if (VSpace >= 2)
                    {
                        writer.EndLine((int)VSpace - 2);
                        writer.ForceWriteLine();
                    }
                }
                else
                    writer.EndLine((int)VSpace-1);
            }
            for (int i = 0; i < HSpace; i++)
                writer.Write(@"\ ");
        }
    }
}
