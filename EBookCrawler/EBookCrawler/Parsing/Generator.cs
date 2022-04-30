using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EBookCrawler.Parsing
{
    public class Generator
    {
        public void WriteHtmlSkeleton(TextWriter writer, IEnumerable<Token> tokens, params string[] attributes)
        {
            int depth = 0;

            void writeIndent()
            {
                for (int i = 0; i < depth; i++)
                    writer.Write(" ");
            }

            void writeAttributes(Token token)
            {
                foreach (var att in token.Attributes)
                    if (attributes.Contains(att.Name))
                        writer.Write(" " + att.Name + "=\"" + att.Value + "\"");
            }

            foreach (var token in tokens)
            {
                if (!token.IsRaw)
                {
                    if (token.IsBeginning)
                    {
                        writeIndent();
                        writer.Write("<");
                        writer.Write(token.Tag);
                        writeAttributes(token);

                        if (token.IsEnd)
                            writer.Write("/>");
                        else
                        {
                            writer.Write(">");
                            depth++;
                        }
                        writer.WriteLine();
                    }
                    else
                    {
                        depth--;
                        writeIndent();
                        writer.Write("</");
                        writer.Write(token.Tag);
                        writer.Write(">");
                        writer.WriteLine();
                    }
                }
            }
        }
    }
}
