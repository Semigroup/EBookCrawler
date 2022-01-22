using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    [Serializable]
    public class BookReference
    {
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public bool PartsHaveDifferentSubTitles { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public string Identifier { get; set; }

        public PartReference[] Parts { get; set; }

        public void SetIdentifer()
        {
            int hash = 0;
            foreach (var part in Parts)
                hash ^= part.Link.GetHashCode();
            this.Identifier = Name + " | " + SubTitle + " | " + Parts.Length + " @" + hash;
        }

        public void WriteBook(string root)
        {
            foreach (var partRef in Parts)
                foreach (var ch in partRef.Part.Chapters)
                {
                    ch.LoadText(root);
                    ch.ParseText();
                }
        }
    }
}
