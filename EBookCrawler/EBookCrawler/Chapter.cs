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
        public struct Meta
        {
            public string Author;
            public string Translator;

            public string Title;
            public string Subtitle;

            public string Year;
            public string FirstPublished;

            public string Type;

            public string Series;
            public string Volume;
            public string Edition;
            public string Editor;
            public string Illustrator;
            public string Publisher;
        }

        public Meta MyMeta { get; set; }
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
        public static readonly Regex MetaRegex = new Regex(
            "<meta\\s+" +
            "name\\s*=\\s*\"(?<name>[^\"]*)\"" +
            "\\s+" +
            "content\\s*=\\s*\"(?<content>[^\"]*)\"" +
            "\\s*/>");

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
        protected void LoadText(string root)
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
            this.MyMeta = ExtractMeta(source);
        }
        public Texting.TextChapter ParseChapter(string root)
        {
            LoadText(root);
            if (Text == null)
                return null;

            File.WriteAllText("text.xml", Text);
            Console.WriteLine(this.URL);

            var tokenizer = new Parsing.Tokenizer();
            tokenizer.Tokenize(Text);

            var rep = new Parsing.Repairer();
            rep.Repair(Text, tokenizer.Tokens);

            var parser = new Parsing.Parser();
            parser.Parse(rep.Output);

            var trafo = new Texting.Transformer();
            var ch = trafo.Transform(URL, Text, parser.Root);
            ch.Chapter = this;

            this.Text = null;

            return ch;
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
        public Meta ExtractMeta(string source)
        {
            Meta meta = new Meta();
            MatchCollection matches = MetaRegex.Matches(source);
            foreach (Match match in matches)
            {
                var groups = match.Groups;
                var name = groups["name"].Value.Trim().ToLower();
                var content = groups["content"].Value.Trim();
                switch (name)
                {
                    case "type":
                        meta.Type = content;
                        break;
                    case "booktitle":
                    case "itle":
                    case "title":
                        meta.Title = content;
                        break;
                    case "subtitle":
                        meta.Subtitle = content;
                        break;
                    case "author":
                    case "auhthor":
                        meta.Author = content;
                        break;
                    case "rtanslator":
                    case "tranlator":
                    case "translator":
                        meta.Translator = content;
                        break;
                    case "year":
                        meta.Year = content;
                        break;
                    case "publisher":
                        meta.Publisher = content;
                        break;
                    case "firstpub":
                        meta.FirstPublished = content;
                        break;
                    case "series":
                    case "iseries":
                        meta.Series = content;
                        break;
                    case "volume":
                        meta.Volume = content;
                        break;
                    case "edition":
                        meta.Edition = content;
                        break;
                    case "editor":
                        meta.Editor = content;
                        break;
                    case "illstrator":
                    case "illustrator":
                    case "tillustrator":
                    case "illustraor":
                    case "illustator":
                    case "ilustrator":
                        meta.Illustrator = content;
                        break;
                    case "keyword":
                    case "quelle":
                    case "source":
                    case "purl":
                    case "submitted":
                    case "generator":
                    case "wgs":
                    case "copyright":
                    case "printrun":
                    case "note":
                    case "comment":
                    case "pages":
                    case "pfad":
                    case "created":
                    case "date":
                    case "modified":
                    case "modifed":
                    case "modfied":
                    case "lastmodified":
                    case "cmodified":
                    case "midified":
                    case "modifieded":
                    case "modyfied":
                    case "modifieed":
                    case "modif´ied":
                    case "modifier":
                    case "projectid":
                    case "corrector":
                    case "corrected":
                    case "2corrected":
                    case "2corrector":
                    case "secondcorrector":
                    case "second corrector":
                    case "secondcorrection":
                    case "secondcorrected":
                    case "thirdcorrector":
                    case "address":
                    case "isbn":
                    case "sender":
                    case "secondsender":
                    case "status":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return meta;
        }
    }
}
