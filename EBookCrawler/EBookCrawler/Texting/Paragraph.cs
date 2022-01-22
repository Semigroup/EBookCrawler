﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
   public class Paragraph : TextElement
    {
        public List<TextElement> Elements { get; set; } = new List<TextElement>();
        /// <summary>
        /// 0 : Left
        /// 5 : Center
        /// 10 : Right
        /// </summary>
        public int Alignment { get; set; }
    }
}