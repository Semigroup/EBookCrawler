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

        public string GetIdentifier()
            => GetIdentifier(this.FirstName, this.LastName);
        public static string GetIdentifier(string FirstName, string LastName)
        {
            return LastName + " | " + FirstName;
        }
        public void MergeBooks()
        {

        }
    }
}
