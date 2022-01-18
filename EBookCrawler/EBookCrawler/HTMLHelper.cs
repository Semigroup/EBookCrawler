using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;

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
            string content = HttpUtility.HtmlDecode(source);
            content = content.Replace("&", "und");
            return content;
        }
        public static string RemoveNewLine(string source) => source.Replace("\n", "").Replace("\r", "");

        public static string ExchangeLastDirectory (string uri, string newDirectory)
        {
            return uri.Substring(0, uri.LastIndexOf('/')+1) + newDirectory;
        }
    }
}
