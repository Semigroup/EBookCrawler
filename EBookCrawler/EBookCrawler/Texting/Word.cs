using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
   public class Word : TextElement
    {
        public FontManager.Mode Mode { get; set; }
        public string Value { get; set; }
    }
}
