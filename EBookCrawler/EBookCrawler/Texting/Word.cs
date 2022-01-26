using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Word : TextElement
    {
        public string Value { get; set; }

        public IEnumerable<Word> SplitLines()
        {
            var lines = Value.Split(new char[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
                yield return new Word() { Value = lines[i] };
        }
    }
}
