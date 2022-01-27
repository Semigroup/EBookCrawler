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
        public Measure Indentation { get; set; }
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
                case "intial":
                case "initial":
                case "initital":
                case "intitial":
                    this.StartsWithCapital = true;
                    break;
                case "fntext":
                    this.Indentation = new Measure("2em");
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
                    this.Indentation = new Measure("2em");
                    break;
                case "drama2":
                case "cdrama2":
                    this.Indentation = new Measure("4em");
                    break;
                case "stage":
                    this.Style = new Style() { IsItalic = true };
                    this.Indentation = new Measure("2em");
                    break;

                case "":
                case "‹h3":
                case "tb":
                case "box":
                case "part":
                case "glossar":
                case "chupter":
                case "section":
                case "dblmarg":
                case "dlbmarg":
                case "dblamrg":
                case "dlmarg":
                case "dblmargr":
                case "dlmargr":
                case "center":
                case "center\"\"":
                case "cent":
                case "cebter":
                case "cemter":
                case "enter":
                case "denter":
                case "stars":
                case "chapter":
                case "motto":
                case "motto50":
                case "prosa":
                case "left":
                case "rleft":
                case "lewft":
                case "leftmarg":
                case "leftmrg":
                case "leftjust":
                case "figure":
                case "right":
                case "riight":
                case "riht":
                case "rightmarg":
                case "signature":
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
                case "versmarg":
                case "weihe":
                case "titel_3":
                case "recipient":
                case "act":
                case "el":
                case "mid":
                case "de":
                case "bigtable":
                case "p5":
                case "p20":
                case "p23":
                case "p24":
                case "p25":
                    break;

                case "not":
                case "note":
                case "small":
                case "centersm":
                case "centersml":
                case "anmerk":
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

                case "smallcaps":
                    this.Style = new Style() { IsSmallCaps = true };
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
