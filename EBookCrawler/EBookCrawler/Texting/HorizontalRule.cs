using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class HorizontalRule : TextElement
    {
        /// <summary>
        /// Proportional. 1 means full Textwidth
        /// </summary>
        public double Length { get; set; } = 1;
    }
}
