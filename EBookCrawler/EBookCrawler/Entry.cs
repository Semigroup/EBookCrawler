using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Text.RegularExpressions;
using Assistment.Extensions;

namespace EBookCrawler
{
    public class Entry
    {
        public static Regex Whitespaces = new Regex("\\s+");

        public class Datum
        {
            public string Value { get; set; }

            public Datum(XmlReader reader)
            {
                this.Value = Whitespaces.Replace(reader.Value.Replace("\n", "").Replace("\r", ""), " ");
                if (this.Value == " ")
                    this.Value = "";
            }
        }

        public enum Kind
        {
            Letter,
            Author,
            Book
        }

        public List<Datum> Data { get; set; } = new List<Datum>();

        public Entry(XmlReader reader)
        {
            bool fresh = true;
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Whitespace)
                {
                    if (fresh)
                        fresh = false;
                    else if (reader.Depth == 0)
                        break;
                    Data.Add(new Datum(reader));
                }
            }
        }

        public (string firsName, string lastName) GetAuthorName()
        {
            string[] authors = Data[1].Value.Split(new char[] { '/' },StringSplitOptions.RemoveEmptyEntries);
            (string firstName, string lastName)[] authorNames = authors.Map(authorName => sepNames(authorName)).ToArray();
            string fName = authorNames[0].firstName;
            string lName = authorNames[0].lastName;
            for (int i = 1; i < authorNames.Length; i++)
            {
                fName += "/" + authorNames[i].firstName;
                lName += "/" + authorNames[i].lastName;
            }
            return (fName, lName);

            (string firstName, string lastName) sepNames(string authorName)
            {
                string[] split = authorName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                switch (split.Length)
                {
                    case 1:
                        return ("", split[0].Trim());
                    case 2:
                        return (split[1].Trim(), split[0].Trim());
                    default:
                        throw new NotImplementedException("Entry.GetAuthorName: " + authorName + " hat zu viele Kommata!");
                }
            }
        }
        public BookReference GetBookReference()
        {
            BookReference br = new BookReference
            {
                Name = Data[2].Value
            };
            if (Data.Count == 10)
                br.Genres.AddRange(Data[7].Value.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));
            return br;
        }
        public Kind GetKind()
        {
            switch (Data.Count)
            {
                case 2:
                    return Kind.Author;
                case 8:
                    return Kind.Letter;
                case 4://Books without Genre
                case 10:
                    return Kind.Book;
                default:
                    throw new NotImplementedException("Entry.GetKind: Encounted " + Data.Count + " Depths!");
            }
            return Kind.Letter;
        }
        public override string ToString()
        {
            return Data.Map(x => x.Value).SumText("|") + ", (" + Data.Map(x => x.Value.Length).SumText("|") + ")";
        }
    }
}
