using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
   public struct Length
    {
        public double Value;
        public bool IsProportional;

        private static string ToString(double value)
            => value.ToString().Replace(',', '.');
        public override string ToString()
        {
            if (IsProportional)
                return ToString(Value) + @"\textwidth{}";
            else
                return ToString(Value) + "pt{}";
        }
        public string GetLeftSpace(TextElement.Alignment alignment)
        {
            switch (alignment)
            {
                case TextElement.Alignment.Unspecified:
                case TextElement.Alignment.Left:
                    return "0pt{}";
                case TextElement.Alignment.Center:
                    if (IsProportional)
                        return ToString((1 - Value)/2) + @"\textwidth{}";
                    else
                        return @"0.5\textwidth{} minus " + ToString(Value / 2) + "pt{}";
                case TextElement.Alignment.Right:
                    if (IsProportional)
                        return ToString(1 - Value) + @"\textwidth{}";
                    else
                        return @"\textwidth{} minus " + ToString(Value) + "pt{}";
                default:
                    throw new NotImplementedException();
            }
        }
        public string GetRightSpace(TextElement.Alignment alignment)
        {
            TextElement.Alignment inv;
            switch (alignment)
            {
                case TextElement.Alignment.Unspecified:
                case TextElement.Alignment.Left:
                    inv = TextElement.Alignment.Right;
                    break;
                case TextElement.Alignment.Center:
                    inv = TextElement.Alignment.Center;
                    break;
                case TextElement.Alignment.Right:
                    inv = TextElement.Alignment.Left;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return GetLeftSpace(inv);
        }
    }
}
