using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Merging
{
    public static class PartMerger
    {
        public static List<BookReference> MergeParts(IEnumerable<PartReference> parts)
        {
            TokenizedTitle[] titles = new TokenizedTitle[parts.Count()];
            for (int i = 0; i < titles.Length; i++)
                titles[i] = new TokenizedTitle(parts.ElementAt(i));

            List<TokenizedTitle> remainingTitles = new List<TokenizedTitle>(titles);

            List<Group> groups = new List<Group>();
            while (remainingTitles.Count > 0)
            {
                Group newGroup = new Group(remainingTitles.First());
                newGroup.ExtractGroup(remainingTitles);
                foreach (var title in newGroup.Titles)
                    remainingTitles.Remove(title);
            }
        }
    }
}
