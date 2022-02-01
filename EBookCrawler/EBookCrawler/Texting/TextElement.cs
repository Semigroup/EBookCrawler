using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public abstract class TextElement
    {
        public enum Alignment
        {
            Unspecified,
            Left,
            Center,
            Right
        }

        public int WordCount { get; set; }
        public bool IsVisible { get; set; } = true;
        //public string Class { get; set; }
        public abstract void ToLatex(LatexWriter writer);

        protected static int CountWords(string text)
        {
            bool isWhitespace(char b)
                => b == ' ' || b == '\n' || b == '\r';

            int i = 0;
            bool lastWS = true;
            foreach (var c in text)
                if (isWhitespace(c))
                    lastWS = true;
                else
                {
                    if (lastWS)
                        i++;
                    lastWS = false;
                }
            return i;
        }
    }
}
