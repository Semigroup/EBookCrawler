using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Extensions;

namespace EBookCrawler
{
    [Serializable]
    public class PartReference
    {
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string Link { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public Part Part { get; set; }

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
        private string TrimSubTitle(string subTitle)
        {
            char[] toTrim = new char[] { ' ', '.', ',' };

            int a = 0;
            while (a < subTitle.Length && toTrim.Contains(subTitle[a]))
                a++;
            if (a == subTitle.Length)
                return "";

            int b = subTitle.Length - 1;
            while (b > a && toTrim.Contains(subTitle[b]))
                b--;
            return subTitle.Substring(a, b - a);
        }
        public void SetSubTitle(string subTitle)
        {
            this.SubTitle = TrimSubTitle(subTitle);
        }
        public void SetGenres(string genres)
        {
            char[] seps = new char[] { ',', '/' };
            this.Genres.AddRange(genres.Split(seps, StringSplitOptions.RemoveEmptyEntries).Map(x => x.Trim()));
        }

        public void Merge(PartReference referenceForSamePart)
        {
            foreach (var genre in referenceForSamePart.Genres)
                if (!this.Genres.Contains(genre))
                    this.Genres.Add(genre);
        }
        public void LoadPart()
        {
            this.Part = new Part(this);
        }
    }
}
