using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace EBookCrawler
{
    [Serializable]
    public class Chapter
    {
        public Part Part { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string RelativePath { get; set; }
        public const string URL_HEAD = "https://www.projekt-gutenberg.org/";
        /// <summary>
        /// zero-based
        /// </summary>
        public int Number { get; set; }

        public string Text { get; set; }
        public bool TextNotFound { get; set; }

        public static readonly Regex HRLine = new Regex(
            "<hr size=\"1\" color=\"#808080\">"
            + ".*"
            + "</hr>");

        public Chapter(Part Part, string Name, string URL, int Number)
        {
            this.Part = Part;
            this.Name = Name;
            this.URL = URL;
            this.RelativePath = URL.Substring(URL_HEAD.Length).Replace('/', '\\');
            this.Number = Number;
        }

        public void DownloadText(string root, bool forceDownload)
        {
            string path = Path.Combine(root, RelativePath);
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            if (forceDownload || !File.Exists(path))
                try
                {
                    string source = HTMLHelper.GetSourceCode(URL);
                    File.WriteAllText(path, source);
                    Console.WriteLine("Written " + path);
                }
                catch (WebException)
                {
                    Logger.LogLine("Couldnt download " + URL);
                }
                catch (IOException)
                {
                    Logger.LogLine("Couldnt write to " + path);
                }
            else
                Console.WriteLine("File already exists: " + path);
        }
        public void LoadText(string root)
        {
            var fp = Path.Combine(root, RelativePath);
            if (!File.Exists(fp))
            {
                Logger.LogLine("File doesnt exist: " + fp);
                TextNotFound = true;
                return;
            }
            string source = File.ReadAllText(fp);
            source = HTMLHelper.RemoveHTMLComments(source);
            this.Text = ExtractParagraphs(source);
        }
        public void ParseText()
        {
            File.WriteAllText("text.xml", Text);
            var tokenizer = new Parsing.Tokenizer();
            tokenizer.Tokenize(Text);
            if (tokenizer.FoundError)
            {
                Console.WriteLine(tokenizer.GetState());
                Console.ReadKey();
            }
            Console.WriteLine("Tokenized " + this.RelativePath);

            var rep = new Parsing.Repairer();
            rep.Repair(tokenizer.Tokens);
            if (rep.FoundError)
                Console.ReadKey();
            var parser = new Parsing.Parser();
            parser.Parse(rep.Output);

            this.Text = null;
            //var parser = new Parsing.Parser();
            //var doc = parser.ParseDocument(tokenizer.Tokens, Text);
            //Console.WriteLine("Parsed " + this.RelativePath);

            //Console.ReadKey();
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
