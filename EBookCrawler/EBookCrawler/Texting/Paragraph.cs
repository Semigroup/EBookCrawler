﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Paragraph : ContainerElement
    {
        public bool StartsWithIndentation { get; set; } = true;
    }
}
