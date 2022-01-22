using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Document : TextElement
    {
        public List<TextElement> TextElements { get; set; } = new List<TextElement>();

        public void Add(TextElement textElement) => TextElements.Add(textElement);
    }
}
