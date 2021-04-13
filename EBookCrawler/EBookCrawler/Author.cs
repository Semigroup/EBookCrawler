using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
   public class Author
    {
        public SortedDictionary<string, BookReference> Books { get; set; } = new SortedDictionary<string, BookReference>();

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
