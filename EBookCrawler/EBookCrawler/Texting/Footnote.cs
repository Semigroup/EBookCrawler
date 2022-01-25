using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBookCrawler.Parsing;

namespace EBookCrawler.Texting
{
    public class Footnote : ContainerElement
    {
        //public Footnote(string tooltip)
        //{
        //    this.Add(Parser.SplitRaw(tooltip, new FontManager.Mode()));
        //}
        public bool IsToolTip { get; set; }
        /// <summary>
        /// Index: 1,*,...
        /// </summary>
        public string Title { get; set; }
    }
}
