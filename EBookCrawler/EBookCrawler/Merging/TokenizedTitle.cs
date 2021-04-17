using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Merging
{
    public class TokenizedTitle : IComparable<TokenizedTitle>
    {
        public int Length => Tokens.Length;
        public string[] Tokens;
        public PartReference Part;
        public int IndexEnumeratingWord = -1;

        public TokenizedTitle(PartReference Part)
        {
            this.Part = Part;
            this.Tokens = Part.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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
                return -1;
            string token = Tokens[IndexEnumeratingWord];
            return TokenConverter.ToNumber(token);
        }

        public bool IsNumber(int tokenIndex) => TokenConverter.ToNumber(Tokens[tokenIndex]) != -1;

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
