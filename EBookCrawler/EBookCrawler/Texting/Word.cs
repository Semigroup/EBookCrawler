using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Word : TextElement
    {
        public string Value { get; private set; }

        public Word(string Value)
        {
            this.Value = Value;
            this.WordCount = CountWords(Value);
        }

        public IEnumerable<Word> SplitLines()
        {
            var lines = Value.Split(new char[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
                yield return new Word(lines[i]);
        }

        public override void ToLatex(LatexWriter writer)
        {
            writer.WriteText(Value);
        }
    }
}
