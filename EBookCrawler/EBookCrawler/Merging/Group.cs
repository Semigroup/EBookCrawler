using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Merging
{
    public class Group
    {
        public List<TokenizedTitle> Titles { get; set; } = new List<TokenizedTitle>();
        public TokenizedTitle Prinzeps { get; set; }

        public Group(TokenizedTitle Prinzeps)
        {
            this.Prinzeps = Prinzeps;
            this.Titles.Add(Prinzeps);
        }

        public void ExtractGroup(List<TokenizedTitle> remainingTitles)
        {
            foreach (var title in remainingTitles)
                if (title.Length == Prinzeps.Length && title.Distance(Prinzeps) == 1)
                {
                    int index = title.GetDifferingWord(Prinzeps);
                    if (title.IsNumber(index) && Prinzeps.IsNumber(index))
                        this.Titles.Add(title);
                }

            if (Titles.Count == 1)
                return;
            int enumIndex = Prinzeps.GetDifferingWord(Titles[1]);
            foreach (var title in Titles)
                title.IndexEnumeratingWord = enumIndex;
            this.Titles.Sort();
        }

        public BookReference GetBookReference()
        {

        }
    }
}
