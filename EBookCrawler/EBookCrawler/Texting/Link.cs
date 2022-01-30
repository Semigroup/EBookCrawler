using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Link : ContainerElement
    {
        public string Name { get; set; }
        public string URL { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public bool IsPageRef { get; set; }

        public override void ToLatex(LatexWriter writer)
        {
            //ToDo
            base.ToLatex(writer);
        }
    }
}
