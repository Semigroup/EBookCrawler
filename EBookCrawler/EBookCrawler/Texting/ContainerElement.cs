using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
   public abstract class ContainerElement : TextElement
    {
        public List<TextElement> TextElements { get; set; } = new List<TextElement>();

        /// <summary>
        /// 0 : Left
        /// 1 : Center
        /// 2 : Right
        /// </summary>
        public int Alignment { get; set; }
        public double Size { get; set; } = 1;
        public bool IsPoem { get; set; }
        public bool IsBox { get; set; }
        public bool IsIndented { get; set; }

        public void Add(TextElement textElement) => TextElements.Add(textElement);
        public void Add(IEnumerable<TextElement> textElements) => TextElements.AddRange(textElements);

        public void SetAlignment(string attValue)
        {
            switch (attValue)
            {
                case "letter":
                    this.IsIndented = true;
                    this.Alignment = 0;
                    break;
                case "abstract":
                    this.IsIndented = true;
                    this.Size = 0.75;
                    this.Alignment = 0;
                    break;
                case "vers":
                    this.Alignment = 1;
                    this.IsPoem = true;
                    break;
                case "dblmarg":
                case "center":
                case "stars":
                    this.Alignment = 1;
                    break;
                case "drama":
                    this.IsIndented = true;
                    this.Alignment = 0;
                    break;
                case "prosa":
                case "left":
                case "leftmarg":
                case "leftjust":
                    this.Alignment = 0;
                    break;
                case "note":
                case "centersml":
                    this.Alignment = 1;
                    this.Size = 0.75;
                    break;
                case "centerbig":
                    this.Alignment = 1;
                    this.Size = 2;
                    break;
                case "figure":
                    this.Alignment = 1;
                    break;
                case "right":
                case "signature":
                    this.Alignment = 2;
                    break;
                case "box":
                    this.Alignment = 1;
                    this.IsBox = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
