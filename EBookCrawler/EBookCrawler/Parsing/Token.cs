﻿using System;
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
            Address
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
                    return new Texting.Length()
                    {
                        Value = double.Parse(Value.Substring(0,Value.Length - 1)),
                        IsProportional = true
                    };
                else
                {
                    double l = double.Parse(Value);
                    return new Texting.Length()
                    {
                        Value = double.Parse(Value.Substring(Value.Length - 1)),
                        IsProportional = 0 <= l && l <= 1
                    };
                }
            }
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

        private void SetKind()
        {
            switch (_Tag)
            {
                case "address":
                    this.MyKind = Kind.Address;
                    break;
                case "raw":
                    this.MyKind = Kind.Raw;
                    break;
                case "p":
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
                case "td":
                    this.MyKind = Kind.TableDatum;
                    break;
                case "h0":
                case "h1":
                case "h2":
                case "h3":
                case "h4":
                case "h5":
                    this.MyKind = Kind.Header;
                    break;
                case "hr":
                    this.MyKind = Kind.HorizontalRuling;
                    break;
                case "span":
                    this.MyKind = Kind.Span;
                    break;
                case "a":
                    this.MyKind = Kind.Link;
                    break;
                case "i":
                    this.MyKind = Kind.Italic;
                    break;
                case "b":
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
