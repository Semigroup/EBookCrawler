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
    }
}
