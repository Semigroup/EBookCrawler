using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Extensions;

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
            TokenizedTitle[] titles = new TokenizedTitle[Books.Values.Count];
            for (int i = 0; i < titles.Length; i++)
                titles[i] = new TokenizedTitle(Books.Values.ElementAt(i));

            List<int> remainingTitles = new List<int>();
            for (int i = 0; i < titles.Length; i++)
                remainingTitles.Add(i);

            List<List<int>> groups = new List<List<int>>();
            while (remainingTitles.Count > 0)
            {
                List<int> newGroup = new List<int>();
                groups.Add(newGroup);
                int index = remainingTitles.First();
                remainingTitles.RemoveAt(0);
                newGroup.Add(index);

                if (titles[index].Length > 2)
                    for (int i = 0; i < titles.Length; i++)
                        if (titles[index].Distance(titles[i]) == 1)
                            if (remainingTitles.Remove(i))
                                newGroup.Add(i);
            }

            foreach (var group in groups)
            {
                var ordered = orderGroup(group);
                //Console.WriteLine("New Group:");
                foreach (var title in ordered)
                {
                    var book = title.Book;
                    //Console.WriteLine("    " + book.Name + ", " + book.SubTitle);
                }
            }
            //Console.ReadKey();

            List<TokenizedTitle> orderGroup(List<int> group)
            {
                if (group.Count == 1)
                    return new List<TokenizedTitle> { titles[group[0]] };
                int index = titles[group[0]].GetDifferingWord(titles[group[1]]);
                List<TokenizedTitle> list = new List<TokenizedTitle>(group.Map(i => titles[i]));
                foreach (var title in list)
                    title.IndexEnumeratingWord = index;
                list.Sort();
                return list;
            }
        }

        private class TokenizedTitle : IComparable<TokenizedTitle>
        {
            public int Length => Tokens.Length;
            public string[] Tokens;
            public BookReference Book;
            public int IndexEnumeratingWord = -1;

            public TokenizedTitle(BookReference Book)
            {
                this.Book = Book;
                this.Tokens = Book.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < Length; i++)
                    this.Tokens[i] = this.Tokens[i].ToLower();
            }

            public int Distance(TokenizedTitle otherTitle)
            {
                int d = 0;
                for (int i = 0; i < Math.Min(this.Length, otherTitle.Length); i++)
                    if (this.Tokens[i] != otherTitle.Tokens[i])
                        d++;
                d += Math.Max(this.Length, otherTitle.Length) - Math.Min(this.Length, otherTitle.Length);
                return d;
            }

            public int GetDifferingWord(TokenizedTitle otherTitle)
            {
                for (int i = 0; i < Math.Min(this.Length, otherTitle.Length); i++)
                    if (this.Tokens[i] != otherTitle.Tokens[i])
                        return i;
                return Math.Min(this.Length, otherTitle.Length) + 1;
            }

            public int GetNumber()
            {
                if (IndexEnumeratingWord >= Tokens.Length)
                    return 0;

                string token = Tokens[IndexEnumeratingWord].Replace(".", "").Replace(",", "");

                if (int.TryParse(token, out int result))
                    return result;

                switch (token)
                {
                    case "erste":
                    case "erster":
                    case "erstes":
                    case "eins":
                    case "i":
                        return 1;

                    case "zweite":
                    case "zweiter":
                    case "zweites":
                    case "zwei":
                    case "ii":
                        return 2;

                    case "dritter":
                    case "drittes":
                    case "dritte":
                    case "drei":
                    case "iii":
                        return 3;

                    case "vierter":
                    case "viertes":
                    case "vierte":
                    case "vier":
                    case "iv":
                        return 4;

                    case "fünfter":
                    case "fünftes":
                    case "fünfte":
                    case "fünf":
                    case "v":
                        return 5;

                    case "sechster":
                    case "sechstes":
                    case "sechste":
                    case "sechs":
                    case "vi":
                        return 6;

                    case "siebter":
                    case "siebtes":
                    case "siebte":
                    case "sieben":
                    case "siebentes":
                    case "vii":
                        return 7;

                    case "achter":
                    case "achtes":
                    case "achte":
                    case "acht":
                    case "viii":
                        return 8;

                    case "neunter":
                    case "neuntes":
                    case "neunte":
                    case "neun":
                    case "ix":
                        return 9;

                    case "zehnter":
                    case "zehntes":
                    case "zehnte":
                    case "zehn":
                    case "x":
                        return 10;

                    case "elfter":
                    case "elftes":
                    case "elf":
                    case "xi":
                        return 11;

                    case "zwölfte":
                    case "xii":
                        return 12;
                    case "dreizehnte":
                    case "xiii":
                        return 13;
                    case "vierzehnte":
                    case "xiv":
                        return 14;
                    case "fünfzehnte":
                    case "xv":
                        return 15;
                    case "sechszehnte":
                    case "sechzehnte":
                    case "xvi":
                        return 16;
                    case "siebenzehnte":
                    case "xvii":
                        return 17;
                    case "achtzehnte":
                    case "xviii":
                        return 18;
                    case "neunzehnte":
                    case "xix":
                        return 19;
                    case "xx":
                        return 20;
                    case "xxi":
                        return 21;
                    case "xxii":
                        return 22;
                    case "xxiii":
                        return 23;
                    case "xxiv":
                        return 24;

                    default:
                        Console.WriteLine("Auther..GetNumber: Unbekannte Nummerierung: " + token);
                        int a = 0;
                        for (int i = 0; i < Math.Min(token.Length, 3); i++)
                            a += token[i] * (1 << (16 - 8 * i));
                        return a;
                        //throw new NotImplementedException("Unbekannte Nummerierung: " + token);
                }
            }

            public int CompareTo(TokenizedTitle other)
            {
                if (this.IndexEnumeratingWord == -1)
                    throw new NotImplementedException();
                if (this.IndexEnumeratingWord != other.IndexEnumeratingWord)
                    throw new NotImplementedException();

                return this.GetNumber() - other.GetNumber();
            }
        }
    }
}
