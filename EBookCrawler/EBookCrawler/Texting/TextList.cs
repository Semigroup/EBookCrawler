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

        public override void Add(TextElement textElement)
        {
            if (textElement is Word word)
            {
                var words = word.SplitLines();
                foreach (var item in words)
                    this.TextElements.Add(item);
            }
            else
                base.Add(textElement);
        }

        public void SetNumbering(string style)
        {
            if (style == "A")
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

        public override void ToLatex(LatexWriter writer)
        {
            WriteBegin(writer);

            ListItem term = null;
            int number = 1;
            if (StartNumber != null)
                number = int.Parse(StartNumber);
            foreach (var item in this.TextElements)
            {
                if (item is ListItem li && li.IsTerm && term == null)
                        term = li;
                else
                {
                    WriteItem(writer, term, item, ref number);
                    term = null;
                }
            }

            WriteEnd(writer);
        }
        protected override void WriteBegin(LatexWriter writer)
        {
            base.WriteBegin(writer);
            writer.WriteLine(@"\begin{enumerate}");
        }
        protected override void WriteEnd(LatexWriter writer)
        {
            writer.WriteLine(@"\end{enumerate}");
            base.WriteEnd(writer);
        }
        protected virtual void WriteItem(LatexWriter writer, ListItem term, TextElement description, ref int number)
        {
            writer.Write(@"    \item[");
            if (term != null)
                term.ToLatex(writer);
            else if (description is ListItem li)
            {
                if (li.Term != null)
                {
                    var t = li.Term;
                    if (int.TryParse(t, out int n))
                    {
                        t = t + ".";
                        number = n + 1;
                    }
                    writer.WriteText(li.Term);
                }
            }
            else
                writer.Write(GetSymbol(Numbering, ref number, IsOrdered));
            writer.Write(@"]");
            if (description != null)
                description.ToLatex(writer);
        }
        protected static string GetSymbol(NumberingType numberingType, ref int number, bool isOrdered)
        {
            switch (numberingType)
            {
                case NumberingType.Stars:
                    return @"$\ast$";
                case NumberingType.Plusses:
                    return @"+";
                case NumberingType.Dashes:
                    return @"-";
                case NumberingType.LongDashes:
                    return @"--";
                case NumberingType.Bullets:
                    return @"$\bullet$";
                case NumberingType.Circles:
                    return @"$\circ";
                case NumberingType.Squares:
                    return @"$\square";
                case NumberingType.ArabicNumbers:
                    var an = number + ".";
                    number++;
                    return an;
                case NumberingType.RomanNumbers:
                    var rn = ToRoman(number);
                    number++;
                    return rn;
                case NumberingType.AlphabeticalSmall:
                    var alph = "(" + ToLowerAlphabetical(number) + ")";
                    number++;
                    return alph;
                case NumberingType.AlphabeticalLarge:
                    var ALPH = "(" + ToLowerAlphabetical(number).ToUpper() + ")";
                    number++;
                    return ALPH;
                case NumberingType.Unspecified:
                    if (isOrdered)
                    {
                        var on = number + ".";
                        number++;
                        return on;
                    }
                    else
                        return @"$\bullet$";
                default:
                    throw new NotImplementedException();
            }
        }
        protected static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }
        protected static string ToLowerAlphabetical(int number)
        {
            string result = "";
            int divisor = 26;
            while (number > 0)
            {
                int remainder = number % divisor;
                number -= remainder;
                number /= divisor;
                result = result + ('a' + remainder);
            }
            return result;
        }
    }
}
