using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Xml;
using System.IO;


namespace EBookCrawler
{
    public static class HTMLHelper
    {
        public static string GetSourceCode(string url)
        {
            string htmlCode;
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                htmlCode = client.DownloadString(url);
            }
            return htmlCode;
        }
        public static string ExtractPart(string source, string startToken, string endToken)
        {
            int a = source.IndexOf(startToken) + startToken.Length;
            int b = source.IndexOf(endToken);

            return source.Substring(a, b - a);
        }
        public static string CleanHTML(string source)
        {
            source = source.Replace("&gt;", "›").Replace("&lt;", "‹");
            string content = HttpUtility.HtmlDecode(source);
            content = content.Replace("&", "und");
            return content;
        }
        public static string RemoveNewLine(string source) => source.Replace("\n", "").Replace("\r", "");

        public static string RemoveDoctype(string source, out string doctype)
        {
            string extractType(int start)
            {
                if (source.Substring(start, 7).ToLower() != "doctype")
                    return null;
                start += 7;
                int end = start;
                while (end )
                {

                }

            }

            int i = 0;
            while (i < source.Length - 10)
            {
                if (source[i] == '<' && source[i+1] == '!')
                {
                    i += 2;
                    if (source.Substring(i, 7).ToLower() == "doctype")
                    {
                        
                    }
                }
            }
        }

        public static string RemoveHTMLComments(string source)
        {
            int pos = 0;

            StringBuilder sb = new StringBuilder();

            bool searchStart()
            {
                while (pos < source.Length)
                {
                    if (source[pos] == '<' && pos + 3 < source.Length && source.Substring(pos, 4) == "<!--")
                        return true;
                    pos++;
                }
                return false;
            }
            bool searchEnd()
            {
                while (pos < source.Length)
                {
                    if (source[pos] == '-' && pos + 2 < source.Length && source.Substring(pos, 3) == "-->")
                        return true;
                    pos++;
                }
                return false;
            }

            int start = 0;
            while (searchStart())
            {
                sb.Append(source.Substring(start, pos - start));
                pos += 4;
                if (searchEnd())
                {
                    pos += 3;
                    start = pos;
                }
                else
                    return sb.ToString();
            }
            sb.Append(source.Substring(start));

            return sb.ToString();
        }
        public static string ExchangeLastDirectory(string uri, string newDirectory)
        {
            return uri.Substring(0, uri.LastIndexOf('/') + 1) + newDirectory;
        }
        public static IEnumerable<Entry> GetEntries(string filenameOfContent)
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
       
    }
}
