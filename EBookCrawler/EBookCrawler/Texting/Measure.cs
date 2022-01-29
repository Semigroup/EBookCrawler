using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public struct Measure
    {
        public string Length { get; set; }
        public Measure(string Length)
        {
            this.Length = Length;
        }

        public bool IsZero()
        {
            if (Length == null)
                return true;
            for (int i = 0; i < Length.Length; i++)
            {
                char c = Length[i];
                if ('1' <= c && c <= '9')
                    return false;
            }
            return true;
        }
    }
}
