using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class HTMLToken : Token
    {
        public struct Attribute
        {
            public string Name;
            public string Value;

            public Attribute(string Name, string Value)
            {
                this.Name = Name;
                this.Value = Value;
            }

            public override string ToString()
            {
                return Name + ": " + Value;
            }

            public double ValueAsDouble() => double.Parse(Value);
        }

        public override bool IsRaw => false;

        public string Tag { get; set; }
        public bool IsBeginning { get; set; }
        public bool IsEnd { get; set; }
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();

        public HTMLToken(Tokenizer tokenizer) : base(tokenizer)
        {

        }
        public HTMLToken(int Position, int Line, int PositionInLine, int Length,
            string Tag, bool IsBeginning, bool IsEnd, List<Attribute> Attributes)
            : base(Position, Line, PositionInLine, Length)
        {
            this.Tag = Tag;
            this.IsBeginning = IsBeginning;
            this.IsEnd = IsEnd;
            this.Attributes = Attributes;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append(Tag + "; ");
            if (IsBeginning)
                sb.Append("B");
            if (IsEnd)
                sb.Append("E");
            sb.Append(": ");
            foreach (var item in Attributes)
                sb.Append(item + ", ");
            return sb.ToString();
        }
        public HTMLToken GetArtificialClosing(HTMLToken position)
            => new HTMLToken(position.Position, position.Line, position.PositionInLine,
                0, this.Tag, false, true, new List<Attribute>());
        public HTMLToken GetArtificialOpening(HTMLToken position)
          => new HTMLToken(position.Position + position.Length, position.Line, position.PositionInLine + position.Length,
              0, this.Tag, true, false, this.Attributes);
    }
}
