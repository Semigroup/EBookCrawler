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
            public string HRef { get; set; }

            public Datum(XmlReader reader)
            {
                this.Value = Whitespaces.Replace(reader.Value.Replace("\n", "").Replace("\r", ""), " ");
                this.HRef = reader.GetAttribute("HREF");
                if (this.HRef != null)
                    this.HRef = Whitespaces.Replace(this.HRef.Replace("\n", "").Replace("\r", ""), " ");
                if (this.Value == " ")
                    this.Value = "";

            }
        }

        public enum Kind
        {
            Letter,
            Author,
            IrregularAuthor,
            Book,
            BookWithBrokenLink,
            Empty
        }

        public int LineNumber { get; set; }
        public List<Datum> Data { get; set; } = new List<Datum>();

        public Entry(XmlReader reader)
        {
            IXmlLineInfo xmlInfo = (IXmlLineInfo)reader;
            this.LineNumber = xmlInfo.LineNumber;
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
            switch (this.GetKind())
            {
                case Kind.Author:
                    return GetManyAuthorsName();
                case Kind.IrregularAuthor:
                    return GetIrregularAuthorName();
                default:
                    throw new NotImplementedException();
            }
        }

        public (string firsName, string lastName) GetIrregularAuthorName()
        {
            string raw = Data[1].Value;
            if (raw.Length == 0)
                raw = Data[2].Value;
            if (raw.Length == 0)
                throw new NotImplementedException();

            string[] split = raw.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            switch (split.Length)
            {
                case 1:
                    return ("", split[0].Trim());
                default:
                    return (split[1].Trim(), split[0].Trim());
            }
        }
        public (string firsName, string lastName) GetManyAuthorsName()
        {
            string[] authors = Data[1].Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
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

        public PartReference GetPartReference()
        {
            PartReference partref = new PartReference
            {
                Name = Data[2].Value,
                Link = Data[1].HRef,
                SubTitle = ""
            };
            partref.RepairLink();
            if (Data.Count == 5)
                partref.SetSubTitle(Data[4].Value);
            if (Data.Count == 7)
            {
                partref.SetSubTitle(Data[4].Value);
                partref.SetGenres(Data[5].Value);
            }
            if (Data.Count == 9 || Data.Count == 10)
            {
                partref.SetSubTitle(Data[4].Value);
                partref.SetGenres(Data[7].Value);
            }
            return partref;
        }
        public Kind GetKind()
        {
            if (Data.Count == 0)
                return Kind.Empty;
            if (Data[1].HRef != null)
                return Kind.Book;
            switch (Data.Count)
            {
                case 2:
                    return Kind.Author;
                case 8:
                    return Kind.Letter;
                //case 4://Books without Genre
                //case 5:
                //case 7:
                //case 9:
                //case 10:
                //    if (Data[1].Value.Length > 0)
                //        return Kind.IrregularAuthor;
                //    return Kind.Book;
                default:
                    if ((Data.Count == 7 || Data.Count == 5) && Data[1].Value.Length > 0)
                        return Kind.IrregularAuthor;
                    else if (Data[2].Value.Length > 0 && Data[1].HRef == null)
                        if (Data.Count == 4)
                            return Kind.IrregularAuthor;
                        else
                            return Kind.BookWithBrokenLink;

                    throw new NotImplementedException("Entry.GetKind: Encounted " + Data.Count + " Depths!");
            }
        }
        public override string ToString()
        {
            return "Line " + LineNumber + " :: " + Data.Map(x => x.Value).SumText("|") + ", (" + Data.Map(x => x.Value.Length).SumText("|") + ")";
        }
    }
}
