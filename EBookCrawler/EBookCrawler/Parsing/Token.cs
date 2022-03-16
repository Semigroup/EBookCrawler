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
            TableHead,
            TableBody,
            TableFoot,
            Header,
            HorizontalRuling,
            Span,
            Link,
            LineBreak,
            Bold,
            Italic,
            Underlined,
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
            ListItem,
            ListTerm,
            Insertion,
            Deletion,
            Address,
            ColumnGroup,
            Quote,
            Column,
            Strike,
            Aside,
            Cite,
            Caption,
            HTML,
            Head,
            Body,
            Style,
            Script,
            Meta
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
            //public double ValueAsPercentage() => double.Parse(Value.Substring(0, Value.Length - 1));
            public Texting.Length ValueAsLength()
            {
                if (Value[Value.Length - 1] == '%')
                {
                    var number = Value.TrimEnd('%');
                    return new Texting.Length()
                    {
                        Value = double.Parse(number) / 100.0,
                        IsProportional = true
                    };
                }
                else
                {
                    double l = double.Parse(Value);
                    return new Texting.Length()
                    {
                        Value = 0.65 * l,
                        IsProportional = 0 <= l && l <= 1
                    };
                }
            }
            public Texting.Measure ValueAsMeasure()
                => new Texting.Measure(Value);
            //public IEnumerable<(string propName, string propValue)> ParseProperties()
            //{
            //    string[] properties = Value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            //    for (int i = 0; i < properties.Length; i++)
            //    {
            //        string[] nv = properties[i].Split(':');
            //        if (nv.Length != 2)
            //            throw new NotImplementedException();
            //        yield return (nv[0].Trim(), nv[1].Trim());
            //    }
            //}
        }

        public int Position { get; set; }
        public int Line { get; set; }
        public int PositionInLine { get; set; }
        public int Length { get; set; }
        /// <summary>
        /// StartPosition of closing tag, if this is opening
        /// </summary>
        public int EndPosition { get; set; } = -1;

        public string Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                this._Tag = value;
                this.SetKind();
            }
        }
        private string _Tag;
        public bool IsBeginning { get; set; }
        public bool IsEnd { get; set; }
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();
        public List<Attribute> StyleProperties { get; set; } = new List<Attribute>();
        public bool HasStyle { get; set; }
        public string Text { get; set; }
        public Kind MyKind { get; set; }

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

        public void ComputeStyle()
        {
            int index = -1;
            for (int i = 0; i < Attributes.Count; i++)
            {
                var att = Attributes[i];
                if (att.Name.ToLower() == "style" && att.Value.Contains(':'))
                {
                    this.HasStyle = true;
                    index = i;
                    string value = att.Value.ToLower();
                    string[] properties = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < properties.Length; j++)
                    {
                        string[] nv = properties[j].Split(':');
                        if (nv.Length != 2)
                            throw new NotImplementedException();
                        this.StyleProperties.Add(new Attribute(nv[0].Trim(), nv[1].Trim()));
                    }
                    break;
                }
            }
            if (HasStyle)
                Attributes.RemoveAt(index);
        }
        private void SetKind()
        {
            switch (_Tag)
            {
                case "caption":
                    this.MyKind = Kind.Caption;
                    break;

                case "cite":
                    this.MyKind = Kind.Cite;
                    break;
                case "aside":
                    this.MyKind = Kind.Aside;
                    break;

                case "strike":
                case "s":
                    this.MyKind = Kind.Strike;
                    break;

                case "q":
                    this.MyKind = Kind.Quote;
                    break;
                case "col":
                    this.MyKind = Kind.Column;
                    break;
                case "colgroup":
                    this.MyKind = Kind.ColumnGroup;
                    break;

                case "address":
                    this.MyKind = Kind.Address;
                    break;
                case "raw":
                    this.MyKind = Kind.Raw;
                    break;
                case "p":
                case "center":
                    this.MyKind = Kind.Paragraph;
                    break;
                case "br":
                    this.MyKind = Kind.LineBreak;
                    break;
                case "table":
                    this.MyKind = Kind.Table;
                    break;
                case "tr":
                    this.MyKind = Kind.TableRow;
                    break;
                case "th":
                case "td":
                    this.MyKind = Kind.TableDatum;
                    break;
                case "thead":
                    this.MyKind = Kind.TableHead;
                    break;
                case "tbody":
                    this.MyKind = Kind.TableBody;
                    break;
                case "tfoot":
                    this.MyKind = Kind.TableFoot;
                    break;

                case "h0":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                case "h6":
                case "h7":
                case "h8":
                    this.MyKind = Kind.Header;
                    break;
                case "hr":
                    this.MyKind = Kind.HorizontalRuling;
                    break;
                case "span":
                    this.MyKind = Kind.Span;
                    break;
                case "a":
                case "link":
                    this.MyKind = Kind.Link;
                    break;
                case "i":
                    this.MyKind = Kind.Italic;
                    break;
                case "b":
                case "strong": //ToDo
                    this.MyKind = Kind.Bold;
                    break;
                case "u":
                    this.MyKind = Kind.Underlined;
                    break;
                case "div":
                    this.MyKind = Kind.Div;
                    break;
                case "img":
                    this.MyKind = Kind.Image;
                    break;
                case "font":
                    this.MyKind = Kind.Font;
                    break;
                case "tt":
                    this.MyKind = Kind.TeleType;
                    break;
                case "em":
                    this.MyKind = Kind.Emphasis;
                    break;
                case "sup":
                    this.MyKind = Kind.Super;
                    break;
                case "sub":
                    this.MyKind = Kind.Sub;
                    break;
                case "pre":
                    this.MyKind = Kind.Verbatim;
                    break;
                case "blockquote":
                    this.MyKind = Kind.BlockQuote;
                    break;
                case "ol":
                case "ul":
                case "dl":
                    this.MyKind = Kind.List;
                    break;
                case "dt":
                    this.MyKind = Kind.ListItem;
                    break;
                case "li":
                case "dd":
                    this.MyKind = Kind.ListItem;
                    break;
                case "big":
                    this.MyKind = Kind.Big;
                    break;
                case "small":
                    this.MyKind = Kind.Small;
                    break;
                case "ins":
                    this.MyKind = Kind.Insertion;
                    break;
                case "del":
                    this.MyKind = Kind.Deletion;
                    break;

                case "html":
                    this.MyKind = Kind.HTML;
                    break;
                case "head":
                    this.MyKind = Kind.Head;
                    break;
                case "body":
                    this.MyKind = Kind.Body;
                    break;

                case "style":
                    this.MyKind = Kind.Style;
                    break;
                case "script":
                    this.MyKind = Kind.Script;
                    break;
                case "meta":
                    this.MyKind = Kind.Meta;
                    break;

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
        public string GetAttribute(string attributName)
        {
            foreach (var item in Attributes)
                if (item.Name == attributName)
                    return item.Value;
            return null;
        }
    }
}
