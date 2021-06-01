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
        public readonly Regex EnumerateIndex = new Regex("<li><a href=\"(?<link>[^\"]*)\">(?<name>[^<]*)</a></li>");

        public PartReference Reference { get; set; }
        public TitlePage TitlePage { get; set; }
        public Chapter[] Chapters { get; set; }

        public Part(PartReference Reference)
        {
            this.Reference = Reference;

            string source = HTMLHelper.GetSourceCode(Reference.Link);
            source.Save("test", "xml");
            source = HTMLHelper.ExtractPart(source, "<div class=\"dropdown-content\"><h4>Inhalt</h4>", "</div></div><a style=\"float: right;\"");
           

            Console.ReadKey();
        }

        private IEnumerable<(string link, string name)> getToc(string source)
        {
            source = HTMLHelper.ExtractPart(source, "<ul>", "</ul>");
            source = HTMLHelper.CleanHTML(source);
            source = HTMLHelper.RemoveNewLine(source);

            MatchCollection matches = EnumerateIndex.Matches(source);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                Console.WriteLine(groups["link"] + " | " + groups["name"]);
            }
        }
    }
}
