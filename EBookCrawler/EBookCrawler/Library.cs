using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Assistment.Extensions;

namespace EBookCrawler
{
    public class Library
    {
        public DateTime TimeStamp { get; set; }
        public SortedDictionary<string, Author> Authors { get; set; } = new SortedDictionary<string, Author>();

        public void Add(Author author) => this.Authors.Add(author.GetIdentifier(), author);

        public void WriteOverviewMarkdown(string filename)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                file.WriteLine("# Library");
                file.WriteLine("# Date: " + TimeStamp);
                foreach (var author in Authors.Values)
                {
                    file.WriteLine();
                    file.WriteLine("## " + author.FirstName + " " + author.LastName + " (" + author.Parts.Count + ")");
                    foreach (var bookref in author.Books.Values)
                    {
                        string line = " - " + bookref.Name;
                        if (bookref.SubTitle != null && bookref.SubTitle.Length > 0)
                            line += ", " + bookref.SubTitle;
                        if (bookref.Genres.Count > 0)
                            line += " (" + bookref.Genres.SumText(", ") + ")";
                        file.WriteLine(line);
                        for (int i = 0; i < bookref.Parts.Length; i++)
                        {
                            var partref = bookref.Parts[i];
                            line = "   " + (i + 1) + ". " + partref.Name;
                            if (bookref.PartsHaveDifferentSubTitles && partref.SubTitle != null && partref.SubTitle.Length > 0)
                                line += ", " + partref.SubTitle;
                            line += " :: [" + partref.Link + "]";
                            file.WriteLine(line);
                        }
                    }
                    //foreach (var partref in author.Parts.Values)
                    //{
                    //    string line = " - " + partref.Name;
                    //    if (partref.SubTitle != null && partref.SubTitle.Length > 0)
                    //        line += ", " + partref.SubTitle;
                    //    if (partref.Genres.Count > 0)
                    //        line += " (" + partref.Genres.SumText(", ") + ")";
                    //    line += " :: [" + partref.Link + "]";
                    //    file.WriteLine(line);
                    //}
                }
            }
        }
        public void Save(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using (FileStream file = File.Create(filename))
                serializer.Serialize(file, this);
        }
    }
}
