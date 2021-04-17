using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    public class BookReference
    {
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public List<string> Genres { get; set; } = new List<string>();

        public PartReference[] Parts { get; set; }


    }
}
