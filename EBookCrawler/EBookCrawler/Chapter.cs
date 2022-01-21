using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace EBookCrawler
{
    public class Chapter
    {
        public Part Part { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

        public static readonly Regex HRLine = new Regex(
            "<hr size=\"1\" color=\"#808080\">"
            + ".*"
            + "</hr>");

        /// <summary>
        /// zero-based
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Text divided in Text Blocks.
        /// </summary>
        public string[] Paragraphs { get; set; }

        public void LoadText()
        {
            var source = HTMLHelper.GetSourceCode(URL);
            File.WriteAllText("source.xml", source);
            var text = ExtractParagraphs(source);
            File.WriteAllText("text.xml", text);
            //Console.ReadKey();

            var parser = new Parsing.Parser();
            parser.Parse(text);
            if (parser.FoundError)
            {
                Console.WriteLine(parser.GetState());
                Console.ReadKey();
            }
        }
        public string ExtractParagraphs(string source)
        {
            var matches = HRLine.Matches(source);
            int start = -1, end = -1; 
            Match preLast = null, last = null;
            foreach (Match match in matches)
            {
                preLast = last;
                last = match;

                
            }
            if (last == null)
            {
                string startTag = "</TABLE> <BR CLEAR=\"all\"> </DIV>";
                 start = source.LastIndexOf(startTag) + startTag.Length;
                 end = source.LastIndexOf("</body>");
            }
            else
            {
                 start = preLast.Index + preLast.Length;
                 end = last.Index;
            }

            var text = HTMLHelper.CleanHTML(source.Substring(start, end - start));

            return text;
        }
    }
}
