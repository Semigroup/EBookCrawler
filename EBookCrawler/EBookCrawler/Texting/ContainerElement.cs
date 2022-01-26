﻿using System;
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
        public double Indentation { get; set; }
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
                case "initial":
                case "intitial":
                    this.StartsWithCapital = true;
                    break;
                case "abstract":
                    this.Indentation = 1;
                    this.Size = -1;
                    break;
                case "figcaption":
                case "figcaptio":
                case "figcation":
                case "date":
                case "epigraph":
                    this.Style = new Style() { IsItalic = true };
                    break;
                case "drama":
                case "drammarg":
                case "cdrama":
                case "drama1":
                case "cdrama1":
                case "letter":
                case "chor":
                case "chormarg":
                case "box":
                    this.Indentation = 1;
                    break;
                case "drama2":
                case "cdrama2":
                    this.Indentation = 2;
                    break;
                case "stage":
                    this.Style = new Style() { IsItalic = true };
                    this.Indentation = 1;
                    break;
                case "dblmarg":
                case "dblamrg":
                case "dlmarg":
                case "dblmargr":
                case "dlmargr":
                case "center":
                case "cebter":
                case "enter":
                case "stars":
                case "chapter":
                case "motto":
                case "prosa":
                case "left":
                case "leftmarg":
                case "leftjust":
                case "figure":
                case "right":
                case "riht":
                case "signature":
                case "font110":
                case "dedication":
                case "scene":
                case "titlepage":
                case "line":
                case "characters":
                case "justify":
                case "reg":
                    break;
                case "note":
                case "centersml":
                    this.Size = -1;
                    break;
                case "centerbig":
                    this.Size = 1;
                    break;
                case "def":
                    this.Style = new Style() { IsBold = true };
                    this.Color = new Color("ff0000");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
