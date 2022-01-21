using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Parsing
{
   public class HTMLElement
    {
        public string Tag { get; set; }
        public List<(string name, string value)> Attributes { get; set; }
        public string Raw { get; set; }
        public bool SelfClosed { get; set; }
        public int Position { get; set; }
        public int LineNumber { get; set; }
        public int PositionInLine { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (SelfClosed)
                sb.AppendLine("<" + Tag + "/>");
            else
                sb.AppendLine("<" + Tag + ">");

            foreach (var (name, value) in Attributes)
                sb.AppendLine(name + ": " + value);

            sb.AppendLine(Raw);

            return sb.ToString();
        }
    }
}
