using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Verbatim : TextElement
    {
        public string Text { get; private set; }

        public Verbatim(string Text)
        {
            this.Text = Text;
            this.WordCount = CountWords(Text);
        }

        public override void ToLatex(LatexWriter writer)
        {
            // Text may not contain "\end{verbatim}"
            if (Text.Contains(@"\end{verbatim}"))
                throw new NotImplementedException();
            writer.BeginEnvironment("verbatim");
            writer.WriteLine();
            writer.WriteLine(Text);
            writer.EndEnvironment("verbatim");
            writer.WriteLine();
        }
    }
}
