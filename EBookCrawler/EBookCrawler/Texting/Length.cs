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

        public override string ToString()
        {
            if (IsProportional)
                return Value + @"\textwidth{}";
            else
                return Value + "pt{}";
        }
        public string GetLeftSpace(int alignment)
        {
            switch (alignment)
            {
                case 0:
                    return "0pt{}";
                case 1:
                    if (IsProportional)
                        return ((1 - Value)/2) + @"\textwidth{}";
                    else
                        return @"0.5\textwidth{} minus " + (Value / 2) + "pt{}";
                case 2:
                    if (IsProportional)
                        return (1 - Value) + @"\textwidth{}";
                    else
                        return @"\textwidth{} minus " + Value + "pt{}";
                default:
                    throw new NotImplementedException();
            }
        }
        public string GetRightSpace(int alignment)
            => GetLeftSpace(2 - alignment);
    }
}
