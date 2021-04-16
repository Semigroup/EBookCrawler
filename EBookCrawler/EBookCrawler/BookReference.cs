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
        public string Link { get; set; }
        //public List<string> PartLinks { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>();

        public string GetIdentifier() => GetIdentifier(this.Name, this.SubTitle, this.Link);
        public static string GetIdentifier(string Name, string SubTitle, string Link)
        {
            return Name + " | " + SubTitle + " @" +Link.GetHashCode() ;//TODO: remove SubTitle here?
        }
        public void RepairLink()
        {
            if (Link.StartsWith("../../"))
                Link = "https://www.projekt-gutenberg.org/" + Link.Substring(6);
        }

        public void Merge(BookReference referenceForSameBook)
        {
            foreach (var genre in referenceForSameBook.Genres)
                if (!this.Genres.Contains(genre))
                    this.Genres.Add(genre);
        }
    }
}
