using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class TextList : ContainerElement
    {
        public enum NumberingType
        {
            Unspecified,
            ArabicNumbers,
            RomanNumbers,
            AlphabeticalSmall,
            AlphabeticalLarge,
            Stars,
            Plusses,
            Dashes,
            LongDashes,
            Bullets,
            Circles,
            Squares
        }

        public bool IsOrdered { get; set; }
        public bool IsDescriptional { get; set; }
        public NumberingType Numbering { get; set; } = NumberingType.Unspecified;
        public string StartNumber { get; set; }

        public void SetNumbering(string style)
        {
            if (style =="A")
            {
                this.Numbering = NumberingType.AlphabeticalLarge;
                return;
            }

            switch (style.ToLower())
            {
                case "a":
                    this.Numbering = NumberingType.AlphabeticalSmall;
                    break;
                case "1":
                    this.Numbering = NumberingType.ArabicNumbers;
                    break;
                case "i":
                    this.Numbering = NumberingType.RomanNumbers;
                    break;
                case "*":
                    this.Numbering = NumberingType.Stars;
                    break;
                case "+":
                    this.Numbering = NumberingType.Plusses;
                    break;
                case "-":
                    this.Numbering = NumberingType.Dashes;
                    break;
                case "circles":
                case "circle":
                case "disc":
                    this.Numbering = NumberingType.Circles;
                    break;
                case "square":
                case "squares":
                    this.Numbering = NumberingType.Squares;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
