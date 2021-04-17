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
                    foreach (var bookref in author.Parts.Values)
                    {
                        string line = " - " + bookref.Name;
                        if (bookref.SubTitle== null || bookref.SubTitle.Length > 0)
                            line += ", " + bookref.SubTitle;
                        if (bookref.Genres.Count > 0)
                            line += " (" + bookref.Genres.SumText(", ") + ")";
                        line += " :: [" + bookref.Link + "]";
                        file.WriteLine(line);
                    }
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
