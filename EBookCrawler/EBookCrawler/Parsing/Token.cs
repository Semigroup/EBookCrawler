using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public class Token
    {
        public enum Kind
        {
            Raw,
            Paragraph,
            Table,
            TableRow,
            TableDatum,
            Header,
            HorizontalRuling,
            Span,
            Link,
            LineBreak,
            Bold,
            Italic,
            Emphasis,
            TeleType,
            Div,
            Image,
            Font,
            Big,
            Small,
            Super,
            Sub,
            Verbatim,
            BlockQuote,
            List,
            ListItem
        }
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


        public int Position { get; set; }
        public int Line { get; set; }
        public int PositionInLine { get; set; }
        public int Length { get; set; }

        public string Tag { get; set; }
        public bool IsBeginning { get; set; }
        public bool IsEnd { get; set; }
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();
        public string Text { get; set; }

        public bool IsRaw => Tag == "raw";

        public Token(Tokenizer tokenizer)
        {
            this.Position = tokenizer.CurrentPosition;
            this.Line = tokenizer.LineNumber;
            this.PositionInLine = tokenizer.PositionInLine;
        }
        public Token(int Position, int Line, int PositionInLine, int Length,
            string Tag, bool IsBeginning, bool IsEnd, List<Attribute> Attributes)
        {
            this.Position = Position;
            this.Line = Line;
            this.PositionInLine = PositionInLine;
            this.Length = Length;

            this.Tag = Tag;
            this.IsBeginning = IsBeginning;
            this.IsEnd = IsEnd;
            this.Attributes = Attributes;
        }

        public Kind GetKind()
        {
            switch (Tag)
            {
                case "raw":
                    return Kind.Raw;
                case "p":
                    return Kind.Paragraph;
                case "br":
                    return Kind.LineBreak;
                case "table":
                    return Kind.Table;
                case "tr":
                    return Kind.TableRow;
                case "td":
                    return Kind.TableDatum;
                case "h0":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                    return Kind.Header;
                case "hr":
                    return Kind.HorizontalRuling;
                case "span":
                    return Kind.Span;
                case "a":
                    return Kind.Link;
                case "i":
                    return Kind.Italic;
                case "b":
                    return Kind.Bold;
                case "div":
                    return Kind.Div;
                case "img":
                    return Kind.Image;
                case "font":
                    return Kind.Font;
                case "tt":
                    return Kind.TeleType;
                case "em":
                    return Kind.Emphasis;
                case "sup":
                    return Kind.Super;
                case "sub":
                    return Kind.Sub;
                case "pre":
                    return Kind.Verbatim;
                case "blockquote":
                    return Kind.BlockQuote;
                case "ol":
                case "ul":
                    return Kind.List;
                case "li":
                    return Kind.ListItem;
                case "big":
                    return Kind.Big;
                case "small":
                    return Kind.Small;

                default:
                    throw new NotImplementedException();
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Pos " + Position
                + "; Line " + Line
                + ", PiL " + PositionInLine
                + "; Length " + Length + ": ");
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
        public Token GetArtificialClosing(Token position)
            => new Token(position.Position, position.Line, position.PositionInLine,
                0, this.Tag, false, true, new List<Attribute>());
        public Token GetArtificialOpening(Token position)
          => new Token(position.Position + position.Length, position.Line, position.PositionInLine + position.Length,
              0, this.Tag, true, false, this.Attributes);
    }
}
