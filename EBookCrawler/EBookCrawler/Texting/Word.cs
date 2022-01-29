using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Word : TextElement
    {
        public string Value { get; set; }

        public IEnumerable<Word> SplitLines()
        {
            var lines = Value.Split(new char[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
                yield return new Word() { Value = lines[i] };
        }

        public override void ToLatex(LatexWriter writer, LatexContext context)
        {
            foreach (var c in Value)
                writer.Write(ReplaceCharacter(c));
        }
        public static string ReplaceCharacter(char c)
        {
            switch (c)
            {
                case '&':
                    return @"\&{}";
                case '%':
                    return @"\%{}";
                case '$':
                    return @"\${}";
                case '#':
                    return @"\#{}";
                case '_':
                    return @"\_{}";
                case '{':
                    return @"\{{}";
                case '}':
                    return @"\}{}";
                case '~':
                    return @"\}textasciitilde{}";
                case '^':
                    return @"\}textasciicircum{}";
                case '\\':
                    return @"\textbackslash{}";
                case '\u00a0': //Non breaking Space
                    return "~";
                default:
                    return c.ToString();
            }
        }
    }
}
