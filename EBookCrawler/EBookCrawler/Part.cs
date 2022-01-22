using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Extensions;
using System.Text.RegularExpressions;
using System.IO;

namespace EBookCrawler
{
    [Serializable]
    public class Part
    {
        public static readonly Regex EnumerateIndex = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "(?<name>[^<]*)</a></li>");
        public static readonly Regex EnumerateIndexWithClassPageRef = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "<a class=\"pageref\"([^<]*)</a>" +
            "(?<name>[^<]*)</a></li>");
        public static readonly Regex EnumerateIndexWithSpan = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "<span class=\"([^\"]*)\">(?<name>[^<]*)</span></a></li>");
        public static readonly Regex EnumerateIndexWithNamePageRef = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "<a name=\"([^\"]*)\"></a><a class=\"pageref\">([^<]*)</a>" +
            "(?<name>[^<]*)</a></li>");
        public static readonly Regex EnumerateIndexWithName = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "<a name=\"([^\"]*)\"></a>" +
            "(?<name>[^<]*)</a></li>");
        public static readonly Regex EnumerateIndexWithIdName = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
           "<a id=\"[^\"]*\" name=\"[^\"]*\"></a>" +
           "(?<name>[^<]*)</a></li>");
        public static readonly Regex EnumerateIndexWithClassPageRefNameSpan = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
           "<a class=\"pageref\" name=\"[^\"]*\">[^<]*</a><span class=\"([^\"]*)\">" +
           "(?<name>[^<]*)</span></a></li>");

        public PartReference Reference { get; set; }
        public Chapter[] Chapters { get; set; }
        public bool NotFound { get; set; }

        public Part(PartReference Reference)
        {
            this.Reference = Reference;

            Console.WriteLine();
            Console.WriteLine(Reference.Link);

            string indexURL = HTMLHelper.ExchangeLastDirectory(Reference.Link, "index.html");
            //Console.WriteLine(indexURL);

            string source;
            try
            {
                source = HTMLHelper.GetSourceCode(indexURL);
            }
            catch (Exception)
            {
                this.NotFound = true;
                Logger.LogLine("Indexdatei nicht gefunden: " + indexURL);
                return;
            }
            source.Save("indexdatei", "xml");
            source = HTMLHelper.ExtractPart(source, "<h3>Inhaltsverzeichnis</h3><br/>", "</p></body>");


            IEnumerable<(string link, string name)> toc = GetToc(source);
            int numbering = 0;
            Chapters = toc.Select(entry => new Chapter()
            {
                Name = entry.name,
                Number = numbering++,
                Part = this,
                URL = HTMLHelper.ExchangeLastDirectory(indexURL, entry.link)
            }).ToArray();


            //foreach (var item in Chapters)
            //{
            //    Console.WriteLine();
            //    Console.WriteLine(item.Number);
            //    Console.WriteLine(item.Name);
            //    Console.WriteLine(item.URL);
            //    item.LoadText();
            //}

            //Console.ReadKey();
        }
        public void SaveChapters(string root)
        {
            if (Chapters == null)
                return;
            foreach (var ch in Chapters)
            {
                string url = ch.URL;
                string source = HTMLHelper.GetSourceCode(url);
                string head = "https://www.projekt-gutenberg.org";
                string path = Path.Combine(root, url.Substring(head.Length));
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                File.WriteAllText(path, source);
                Console.WriteLine("Written " + path);
            }
        }

        private IEnumerable<(string link, string name)> GetToc(string source)
        {
            source = HTMLHelper.ExtractPart(source, "<ul>", "</ul>");
            source = HTMLHelper.CleanHTML(source);
            source = HTMLHelper.RemoveNewLine(source);

            MatchCollection matches = EnumerateIndex.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithClassPageRef.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithSpan.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithNamePageRef.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithName.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithIdName.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithClassPageRefNameSpan.Matches(source);
            if (matches.Count == 0)
                throw new NotImplementedException();

            foreach (Match match in matches)
            {
                var groups = match.Groups;
                yield return (groups["link"].Value.Trim(), groups["name"].Value.Trim());
            }
            yield break;
        }
    }
}
