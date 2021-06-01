using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
   public class Chapter
    {
        public Part Part { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// zero-based
        /// </summary>
        public int Number { get; set; }
        ///// <summary>
        ///// Context divided in Text Blocks.
        ///// </summary>
        //public string[] TextBlocks;
    }
}
