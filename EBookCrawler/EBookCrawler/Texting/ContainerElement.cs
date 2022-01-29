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

        /// <summary>
        /// 0 : Left
        /// 1 : Center
        /// 2 : Right
        /// </summary>
        public int Alignment { get; set; }
        public int Size { get; set; }
        public Measure LeftMargin { get; set; }
        public Color Color { get; set; }
        public Style Style { get; set; }
        public bool StartsWithCapital { get; set; }

        public virtual void Add(TextElement textElement) => TextElements.Add(textElement);
        public virtual void Add(IEnumerable<TextElement> textElements) => TextElements.AddRange(textElements);

        public virtual void SetClass(string classValue)
        {
            if (classValue == null)
                return;
            this.Alignment = Transformer.GetAlignment(classValue);
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
        public override void ToLatex(LatexWriter writer)
        {
            writer.WriteLine(@"{");
            if (!LeftMargin.IsZero())
                writer.WriteLine(@"\begin{adjustwidth}{" + LeftMargin.Length + "}{}");
            writer.WriteAlignment(Alignment);
            writer.WriteSize(Size);
            if (!Color.IsBlack())
                writer.WriteColor(Color);
            if (StartsWithCapital)
                writer.StartWithCapital = true;

            writer.PushStyle(Style);
            writer.WriteStyle();

            foreach (var element in TextElements)
            {
                element.ToLatex(writer);
                writer.WriteLine();
            }

            writer.PopStyle();

            if (!LeftMargin.IsZero())
                writer.WriteLine(@"\end{adjustwidth}");
            writer.WriteLine(@"}");
        }
    }
}
