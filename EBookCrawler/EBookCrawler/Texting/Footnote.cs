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
        /// <summary>
        /// Inhalt ist Superindex. Footnote ist in Title
        /// Inhalte sollte nicht dargestellt werden...
        /// </summary>
        public bool IsSideNote { get; set; }
        /// <summary>
        /// Inhalt ist Wort, zu dem die Footnote gehört. Footnote ist in Title
        /// </summary>
        public bool IsToolTip { get; set; }
        /// <summary>
        /// Index: 1,*,...
        /// </summary>
        public string Title { get; set; }
    }
}
