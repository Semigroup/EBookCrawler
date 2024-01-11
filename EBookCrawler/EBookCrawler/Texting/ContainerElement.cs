using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class ContainerElement : TextElement
    {
        public List<TextElement> TextElements { get; set; } = new List<TextElement>();

        public Alignment MyAlignment { get; set; } = Alignment.Unspecified;
        public int? Size { get; set; }
        public Measure LeftMargin { get; set; }
        public Color? Color { get; set; }
        public Style Style { get; set; }
        public bool StartsWithCapital { get; set; }
        protected virtual bool SingleLine => WordCount <= 5 && !HasExteriorEnvironment();

        public virtual void Add(TextElement textElement)
        {
            TextElements.Add(textElement);
            WordCount += textElement.WordCount;
        }
        public virtual void Add(IEnumerable<TextElement> TextElements)
        {
            foreach (var item in TextElements)
                Add(item);
        }
        public virtual void SetClass(string classValue)
        {
            if (classValue == null)
                return;
            if (classValue == "anzeige-chap")
            {
                this.IsVisible = false;
                return;
            }
            this.MyAlignment = Transformer.GetAlignment(classValue);
            switch (classValue)
            {
                case "vinit":
                case "intial":
                case "intital":
                case "initial":
                case "inital":
                case "initital":
                case "intitial":
                case "first":
                    this.StartsWithCapital = true;
                    break;
                case "fntext":
                    this.LeftMargin = new Measure("2em");
                    this.Size = -2;
                    break;
                case "abstract":
                case "abstract2":
                case "abstract3":
                    //this.Indentation = 1;
                    this.Style = new Style() { IsItalic = true };
                    this.Size = -1;
                    break;
                case "figcaption":
                case "figcaptio":
                case "figcation":
                case "caption":
                case "date":
                case "address":
                case "epigraph":
                case "regie":
                case "sender":
                    this.Style = new Style() { IsItalic = true };
                    break;
                case "drama":
                case "drma":
                case "drammarg":
                case "cdrama":
                case "drama1":
                case "cdrama1":
                case "letter":
                case "chor":
                case "chormarg":
                case "leftmarg":
                case "leftmarg2":
                case "leftmrg":
                case "rightmarg":
                case "versmarg":
                case "lat":
                case "indented":
                    this.LeftMargin = new Measure("2em");
                    break;
                case "drama2":
                case "cdrama2":
                case "dblmarg":
                case "dbkmarg":
                case "dlbmarg":
                case "dblamrg":
                case "dlmarg":
                case "dllmarg":
                case "dblmargr":
                case "dlmargr":
                    this.LeftMargin = new Measure("4em");
                    break;
                case "stage":
                    this.Style = new Style() { IsItalic = true };
                    this.LeftMargin = new Measure("2em");
                    break;

                case "":
                case "‹h3":
                case "tb":
                case "box":
                case "poem":
                case "einr":
                case "einr1":
                case "calibre13":
                case "volume":
                case "part":
                case "glossar":
                case "chupter":
                case "section":
                case "preface":
                case "center":
                case "center0":
                case "centr":
                case "center\"\"":
                case "cent":
                case "cebter":
                case "cemter":
                case "enter":
                case "denter":
                case "fall":
                case "repliccont":
                case "blockquote":
                case "stars":
                case "chapter":
                case "motto":
                case "motto50":
                case "prosa":
                case "left":
                case "left0":
                case "ldeft":
                case "rleft":
                case "lewft":
                case "leftjust":
                case "figure":
                case "right":
                case "riight":
                case "riht":
                case "signature":
                case "sinature":
                case "signatur":
                case "font110":
                case "dedication":
                case "scene":
                case "titlepage":
                case "line":
                case "iniline":
                case "characters":
                case "justify":
                case "reg":
                case "western":
                case "v":
                case "d":
                case "weihe":
                case "long":
                case "titel_3":
                case "recipient":
                case "act":
                case "bündig":
                case "allname":
                case "el":
                case "mid":
                case "de":
                case "bigtable":
                case "paul simmel":
                case "rolle":
                case "p5":
                case "p20":
                case "p23":
                case "p24":
                case "p25":
                case "true":
                    break;

                case "not":
                case "note":
                case "small":
                case "centersm":
                case "centersmall":
                case "centersml":
                case "anmerk":
                case "anm":
                case "footnote":
                    this.Size = -1;
                    break;

                case "centerbig":
                case "centerbib":
                    this.Size = 1;
                    break;
                case "c1781":
                    this.Color = new Color("0000ff");
                    break;
                case "def":
                    this.Style = new Style() { IsBold = true };
                    this.Color = new Color("ff0000");
                    break;
                case "speaker":
                    this.Style = new Style() { IsBold = true };
                    break;

                case "wide":
                    this.Style = new Style() { IsWide = true };
                    break;

                case "smallcaps":
                    this.Style = new Style() { IsSmallCaps = true };
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual bool HasExteriorEnvironment()
        {
            if (!LeftMargin.IsZero())
                return true;
            return MyAlignment != Alignment.Unspecified;
        }

        public override void ToLatex(LatexWriter writer)
        {
            if (TextElements.Count == 0)
                return;
            if(!IsVisible)
                return;
            WriteBegin(writer);
            foreach (var element in TextElements)
            {
                element.ToLatex(writer);
                //if (!SingleLine)
                //    writer.WriteLine();
            }
            WriteEnd(writer);
        }
        protected virtual void WriteBegin(LatexWriter writer)
        {
            if (!SingleLine)
                writer.WriteLine();
            if (HasExteriorEnvironment())
            {
                if (!LeftMargin.IsZero())
                    writer.BeginEnvironment("adjustwidth", LeftMargin.Length, "");
                writer.WriteBeginAlignment(MyAlignment);
            }
            else
                writer.Write("{");
            if (!SingleLine)
                writer.WriteLine();

            writer.WriteSize(Size);
            writer.WriteColor(Color);
            if (StartsWithCapital)
                writer.StartWithCapital = true;

            writer.PushStyle(Style);
            writer.WriteStyle();
            if (!SingleLine)
                writer.WriteLine();
            else
                writer.Write(" ");
        }
        protected virtual void WriteEnd(LatexWriter writer)
        {
            writer.PopStyle();

            if (!SingleLine)
                writer.WriteLine();
            if (!HasExteriorEnvironment())
                writer.Write("}");
            else
            {
                writer.WriteEndAlignment(MyAlignment);
                if (!LeftMargin.IsZero())
                    writer.EndEnvironment("adjustwidth");
            }
            if (!SingleLine)
                writer.WriteLine();
        }
    }
}
