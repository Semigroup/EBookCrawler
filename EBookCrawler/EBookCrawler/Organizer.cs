using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using Assistment.Extensions;
using System.IO;
using System.Web;

namespace EBookCrawler
{
    /// <summary> 
    /// Processes the index site: https://www.projekt-gutenberg.org/info/texte/allworka.html
    /// </summary>
    public class Organizer
    {
        public string IndexSiteURL { get; set; } = "https://www.projekt-gutenberg.org/info/texte/allworka.html";

        public void LoadLibrary()
        {
            Library lib = new Library()
            {
                TimeStamp = DateTime.Now
            };

            string htmlCode = HTMLHelper.GetSourceCode(IndexSiteURL);
            string fileName = saveDLContent(htmlCode);
            FillLibrary(lib, fileName);

            foreach (var item in lib.Authors.Values)
                item.MergeParts();

            lib.WriteOverviewMarkdown("library_overview.md");
            foreach (var author in lib.Authors.Values)
                foreach (var book in author.Books.Values)
                    foreach (var part in book.Parts)
                    {
                        part.GetPart();
                    }
        }
        private void FillLibrary(Library library, string filenameOfContent)
        {
            Author currentAuthor = null;
            foreach (var entry in getEntries(filenameOfContent))
            {
                switch (entry.GetKind())
                {
                    case Entry.Kind.Letter:
                        break;
                    case Entry.Kind.Author:
                        {
                            (string firstName, string lastName) = entry.GetAuthorName();
                            string identifier = Author.GetIdentifier(firstName, lastName);
                            if (!library.Authors.TryGetValue(identifier, out currentAuthor))
                            {
                                currentAuthor = new Author()
                                {
                                    FirstName = firstName,
                                    LastName = lastName
                                };
                                library.Add(currentAuthor);
                            }
                            else
                                Console.WriteLine("Found the same author twice: " + entry);
                        }
                        break;
                    case Entry.Kind.IrregularAuthor:
                        Console.WriteLine("Irregular Author: " + entry);
                        {
                            (string firstName, string lastName) = entry.GetAuthorName();
                            string identifier = Author.GetIdentifier(firstName, lastName);
                            if (!library.Authors.TryGetValue(identifier, out currentAuthor))
                            {
                                currentAuthor = new Author()
                                {
                                    FirstName = firstName,
                                    LastName = lastName
                                };
                                library.Add(currentAuthor);
                            }
                        }
                        break;
                    case Entry.Kind.Book:
                        PartReference bookRef = entry.GetPartReference();
                        string bookIdentifier = bookRef.GetIdentifier();
                        if (currentAuthor.Parts.TryGetValue(bookIdentifier, out PartReference foundBookRef))
                        {
                            foundBookRef.Merge(bookRef);
                            Console.WriteLine("Found the same book twice: " + entry);
                        }
                        else
                            currentAuthor.Parts.Add(bookIdentifier, bookRef);
                        break;
                    case Entry.Kind.Empty:
                        Console.WriteLine("Empty Entry: " + entry);
                        break;
                    case Entry.Kind.BookWithBrokenLink:
                        Console.WriteLine("Book broken Link: " + entry);
                        break;
                    default:
                        throw new NotImplementedException("Organizer.LoadLibrary(): New Kind " + entry.GetKind() + " !");
                }
            }
        }
        private IEnumerable<Entry> getEntries(string filenameOfContent)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlReader reader = XmlReader.Create(File.OpenText(filenameOfContent), settings);
            while (!reader.EOF)
            {
                Entry e = new Entry(reader);
                yield return e;
            }
            yield break;
        }
        private string saveDLContent(string htmlCode)
        {
            string content = HTMLHelper.ExtractPart(htmlCode, "<DL>", "</DL>");
            content = HTMLHelper.CleanHTML(content);
            content = content.Replace("</a>", "</A>");
            content = content.Replace("<BR>", "");
            content = content.Replace("<<", "<");

            string fileName = "contentFile";
            string extension = "xml";
            content.Save(fileName, extension);
            return fileName + "." + extension;
        }
    }
}
