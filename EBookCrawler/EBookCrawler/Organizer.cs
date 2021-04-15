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
            Library lib = new Library();

            string htmlCode;
            using (WebClient client = new WebClient())
                htmlCode = client.DownloadString(IndexSiteURL);
            string fileName = saveDLContent(htmlCode);
            FillLibrary(lib, fileName);
           
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
                        break;
                    case Entry.Kind.Book:
                        BookReference bookRef = entry.GetBookReference();
                        currentAuthor.Books.Add(bookRef.GetIdentifier(), bookRef);
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
            string startBracket = "<DL>";
            string endBracket = "</DL>";
            int a = htmlCode.IndexOf(startBracket) + 4;
            int b = htmlCode.IndexOf(endBracket);

            string content = htmlCode.Substring(a, b - a);
            content = HttpUtility.HtmlDecode(content);
            content = content.Replace("<BR>", "");
            content = content.Replace("&", "und");
            content = content.Replace("</a>", "</A>");

            //content = content.Replace("&auml;", "ä");
            //content = content.Replace("&Auml;", "Ä");
            //content = content.Replace("&euml;", "ë");
            //content = content.Replace("&iuml;", "ï");
            //content = content.Replace("&ouml;", "ö");
            //content = content.Replace("&Ouml;", "Ö");
            //content = content.Replace("&uuml;", "ü");
            //content = content.Replace("&Uuml;", "Ü");

            //content = content.Replace("&szlig;", "ß");

            //content = content.Replace("&eacute;", "é");
            //content = content.Replace("&Eacute;", "É");

            //content = content.Replace("&agrave;", "à");
            //content = content.Replace("&egrave;", "è");

            //content = content.Replace("&oslash;", "ø");

            //content = content.Replace("&raquo;", "»");
            //content = content.Replace("&laquo;", "«");

            //content = content.Replace("&ntilde;", "ñ");
            //content = content.Replace("&ocirc;", "ô");

            string fileName = "contentFile";
            string extension = "xml";
            content.Save(fileName, extension);
            return fileName + "." + extension;
            //return content.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        }
    }
}
