using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.IO;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace EBookCrawler
{
    /// <summary> 
    /// Processes the index site: https://www.projekt-gutenberg.org/info/texte/allworka.html
    /// </summary>
    public class Organizer
    {
        public string IndexSiteURL { get; set; } = "https://www.projekt-gutenberg.org/info/texte/allworka.html";
        public Library Library { get; set; }
        public string Root { get; set; }

        public Organizer(string Root)
        {
            this.Root = Root;
        }

        public void DownloadLibrary()
        {
            Library = new Library()
            {
                TimeStamp = DateTime.Now
            };

            string htmlCode = HTMLHelper.GetSourceCode(IndexSiteURL);
            string fileName = "contentFile.xml";
            SaveDLContent(htmlCode, fileName);
            FillLibrary(Library, fileName);

            foreach (var item in Library.Authors.Values)
                item.MergeParts();

            foreach (var author in Library.Authors.Values)
                foreach (var book in author.Books.Values)
                    foreach (var part in book.Parts)
                        part.LoadPart();
        }

        public void SaveLibrary()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(Path.Combine(Root, "library.file"), FileMode.Create))
                formatter.Serialize(fs, Library);
            Library.WriteOverviewMarkdown(Path.Combine(Root, "overview.md"));
        }
        public void LoadLibrary()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(Path.Combine(Root, "library.file"), FileMode.Open))
                Library = formatter.Deserialize(fs) as Library;
        }
        public void DownloadWebPage(bool forceDownload)
        {
            Library.DownloadChapters(Root, forceDownload);
        }

        private static void SaveDLContent(string htmlCode, string path)
        {
            string content = HTMLHelper.ExtractPart(htmlCode, "<DL>", "</DL>");
            content = HTMLHelper.CleanHTML(content);
            content = content.Replace("</a>", "</A>");
            content = content.Replace("<BR>", "");
            content = content.Replace("<<", "<");

            File.WriteAllText(path, content);
        }
        private void FillLibrary(Library library, string filenameOfContent)
        {
            Author currentAuthor = null;
            foreach (var entry in HTMLHelper.GetEntries(filenameOfContent))
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
                                Logger.LogLine("Found the same author twice: " + entry);
                        }
                        break;
                    case Entry.Kind.IrregularAuthor:
                        Logger.LogLine("Irregular Author: " + entry);
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
                            Logger.LogLine("Found the same book twice: " + entry);
                        }
                        else
                            currentAuthor.Parts.Add(bookIdentifier, bookRef);
                        break;
                    case Entry.Kind.Empty:
                        Logger.LogLine("Empty Entry: " + entry);
                        break;
                    case Entry.Kind.BookWithBrokenLink:
                        Logger.LogLine("Book broken Link: " + entry);
                        break;
                    default:
                        throw new NotImplementedException("Organizer.LoadLibrary(): New Kind " + entry.GetKind() + " !");
                }
            }
        }
    }
}
