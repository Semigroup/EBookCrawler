using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EBookCrawler
{
   public class Chapter
    {
        public Part Part { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

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
            var text = HTMLHelper.ExtractBiggestPart(source, "weiter&nbsp;&gt;&gt;</a>&nbsp;</hr>", "<hr size=\"1\" color=\"#808080\">&nbsp;");
            text = HTMLHelper.CleanHTML(text);
            File.WriteAllText(Name + ".xml", text);
        }
    }
}
