using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Extensions;
using System.Text.RegularExpressions;


namespace EBookCrawler
{
    public class Part
    {
        public readonly Regex EnumerateIndex = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "(?<name>[^<]*)</a></li>");
        public readonly Regex EnumerateIndexWithPage = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "<a class=\"pageref\"([^<]*)</a>" + 
            "(?<name>[^<]*)</a></li>");
        public readonly Regex EnumerateIndexWithSpan = new Regex("<li><a href=\"(?<link>[^\"]*)\">" +
            "<span class=\"upper\">(?<name>[^<]*)</span></a></li>");

        public PartReference Reference { get; set; }
        public TitlePage TitlePage { get; set; }
        public Chapter[] Chapters { get; set; }
        public bool NotFound { get; set; }

        public Part(PartReference Reference)
        {
            this.Reference = Reference;

            Console.WriteLine();
            Console.WriteLine(Reference.Link);

            string indexURL = HTMLHelper.ExchangeLastDirectory(Reference.Link, "index.html");
            Console.WriteLine(indexURL);

            string source;
            try
            {
                source = HTMLHelper.GetSourceCode(indexURL);
            }
            catch (Exception)
            {
                this.NotFound = true;
                Console.WriteLine("Indexdatei nicht gefunden: " + indexURL);
                Console.ReadKey();
                return;
            }
            source.Save("test", "xml");
            //source = HTMLHelper.ExtractPart(source, "<div class=\"dropdown-content\"><h4>Inhalt</h4>", "</div></div><a style=\"float: right;\"");
            source = HTMLHelper.ExtractPart(source, "<h3>Inhaltsverzeichnis</h3><br/>", "</p></body>");


            IEnumerable<(string link, string name)> toc = getToc(source);
            foreach (var entry in toc)
                Console.WriteLine(entry.name + " | " + entry.link);

            //Console.ReadKey();
        }

        private IEnumerable<(string link, string name)> getToc(string source)
        {
            source = HTMLHelper.ExtractPart(source, "<ul>", "</ul>");
            source = HTMLHelper.CleanHTML(source);
            source = HTMLHelper.RemoveNewLine(source);

            MatchCollection matches = EnumerateIndex.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithPage.Matches(source);
            if (matches.Count == 0)
                matches = EnumerateIndexWithSpan.Matches(source);
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
