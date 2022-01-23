using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class TextList : TextElement
    {
        public class Item : ContainerElement
        {

        }

        public bool IsOrdered { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();

        public void Add(IEnumerable<TextElement> elements)
        {
            foreach (var item in elements)
                if (item is Item)
                    Items.Add(item as Item);
                else
                    throw new NotImplementedException();
        }
    }
}
