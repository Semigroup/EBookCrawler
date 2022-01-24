using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
    public abstract class Token
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
        public Token(int Position, int Line, int PositionInLine, int Length)
        {
            this.Position = Position;
            this.Line = Line;
            this.PositionInLine = PositionInLine;
            this.Length = Length;
        }

        public Kind GetKind()
        {
            if (IsRaw)
                return Kind.Raw;
            string tag = (this as HTMLToken).Tag.ToLower();
            switch (tag)
            {
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
            return "Pos " + Position + "; Line " + Line + ", PiL " + PositionInLine + "; Length " + Length + ": "; 
        }
    }
}
