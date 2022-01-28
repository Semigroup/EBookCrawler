using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Paragraph : ContainerElement
    {
        public bool StartsWithIndentation { get; set; } = true;
        public override void SetClass(string classValue)
        {
            switch (classValue)
            {
                case "line":
                case "iniline":
                case "hanging":
                case "lzeile":
                case "noindent":
                    this.StartsWithIndentation = false;
                    break;
                case "rzeile":
                case "indent":
                    break;
                default:
                    base.SetClass(classValue);
                    break;
            }
        }
    }
}
