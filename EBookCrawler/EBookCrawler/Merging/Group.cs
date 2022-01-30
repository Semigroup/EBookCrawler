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
            for (int i = 0; i < Titles.Count; i++)
                Titles[i].Part.Number = i;
        }

        private bool IsHelpingWord(string word)
        {
            word = word.ToLower();
            if (word.Length == 1)
            {
                char letter = word[0];
                if ('0' <= letter && letter <= '9')
                    return false;
                if ('a' <= letter && letter <= 'z')
                    return false;
                return true;
            }
            if (word == "band")
                return true;
            if (word == "teil")
                return true;
            return false;
        }
        private string TrimPeriod(string word)
        {
            char[] periods = { ',', '.' };
            char lastLetter = word[word.Length - 1];
            if (periods.Contains(lastLetter))
                return word.Substring(0, word.Length - 1);
            return word;
        }

        public BookReference GetBookReference()
        {
            BookReference bookReference = new BookReference
            {
                Parts = this.Titles.Map(title => title.Part).ToArray(),
                SubTitle = ""
            };

            int max = Prinzeps.IndexEnumeratingWord;
            if (max < 0)
                bookReference.Name = Prinzeps.Part.Name;
            else
            {
                string[] words = Prinzeps.Part.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                while (max > 0 && IsHelpingWord(words[max - 1]))
                    max--;

                bookReference.Name = words[0];
                for (int i = 1; i < max - 1; i++)
                    bookReference.Name += " " + words[i];

                if (max > 1)
                    bookReference.Name += " " + TrimPeriod(words[max - 1]);
                else
                    bookReference.Name = TrimPeriod(words[0]);
            }

            foreach (var title in Titles)
                foreach (var genre in title.Part.Genres)
                    if (!bookReference.Genres.Contains(genre))
                        bookReference.Genres.Add(genre);
            bookReference.Genres.Sort();

            foreach (var title in Titles)
            {
                string sub = title.Part.SubTitle;
                if (sub.Length > 0)
                {
                    if (bookReference.SubTitle.Length > 0)
                    {
                        if (bookReference.SubTitle != sub)
                        {
                            bookReference.SubTitle = "";
                            bookReference.PartsHaveDifferentSubTitles = true;
                            break;
                        }
                    }
                    else
                        bookReference.SubTitle = sub;
                }
            }

            bookReference.SetIdentifer();

            return bookReference;
        }
    }
}
