using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    public class Library
    {
        public SortedDictionary<string, Author> Authors { get; set; } = new SortedDictionary<string, Author>();

        public void Add(Author author) => this.Authors.Add(author.GetIdentifier(), author);
    }
}
