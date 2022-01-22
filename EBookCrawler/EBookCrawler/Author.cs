using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Extensions;
using EBookCrawler.Merging;

namespace EBookCrawler
{
    [Serializable]
    public class Author
    {
        public SortedDictionary<string, PartReference> Parts { get; set; } = new SortedDictionary<string, PartReference>();
        public SortedDictionary<string, BookReference> Books { get; set; } = new SortedDictionary<string, BookReference>();

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string GetIdentifier()
            => GetIdentifier(this.FirstName, this.LastName);
        public static string GetIdentifier(string FirstName, string LastName)
        {
            return LastName + " | " + FirstName;
        }
        public void MergeParts()
        {
            List<BookReference> Books = new List<BookReference>(PartMerger.MergeParts(Parts.Values));
            foreach (var book in Books)
                this.Books.Add(book.Identifier, book);
        }
    }
}
