﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Poem : ContainerElement
    {
        public override bool HasExteriorEnvironment()
            => true;

        public override void ToLatex(LatexWriter writer)
        {
            writer.BeginEnvironment("quote");
            base.ToLatex(writer);
            writer.EndEnvironment("quote");
            writer.WriteLine();
        }
    }
}
