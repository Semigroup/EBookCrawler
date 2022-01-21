using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public abstract class Token
    {
        public abstract bool IsRaw { get; }

        public int Position { get; set; }
        public int Line { get; set; }
        public int PositionInLine { get; set; }
        public int Length { get; set; }

        public Token(Tokenizer tokenizer)
        {
            this.Position = tokenizer.CurrentPosition;
            this.Line = tokenizer.LineNumber;
            this.PositionInLine = tokenizer.PositionInLine;
        }

        public override string ToString()
        {
            return "Pos " + Position + "; Line " + Line + ", PiL " + PositionInLine + "; Length " + Length + ": "; 
        }
    }
}
