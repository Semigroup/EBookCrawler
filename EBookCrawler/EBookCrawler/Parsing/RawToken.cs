using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class RawToken : Token
    {
        public override bool IsRaw => true;

        public string Text { get; set; }

        public RawToken(Tokenizer tokenizer) : base(tokenizer)
        {

        }

        public override string ToString()
        {
            return base.ToString() + Text;
        }
    }
}
