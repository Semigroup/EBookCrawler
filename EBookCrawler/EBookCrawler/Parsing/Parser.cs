using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBookCrawler.Texting;

namespace EBookCrawler.Parsing
{
    public class Parser
    {
        public Token[] Tokens { get; set; }
        public int CurrentPosition { get; set; }
        public Token CurrentToken => Tokens[CurrentPosition];
        public HTMLToken HTMLToken => CurrentToken as HTMLToken;
        public Document Document { get; set; }

        public void ParseDocument(IEnumerable<Token> tokens)
        {
            this.Tokens = tokens.ToArray();
            this.Document = new Document();

            while (CurrentPosition < Tokens.Length)
            {
                switch (CurrentToken.GetKind())
                {
                    case Token.Kind.Raw:
                        break;
                    case Token.Kind.Paragraph:
                    case Token.Kind.Table:
                    case Token.Kind.Header:
                    case Token.Kind.HorizontalRuling:
                        Document.Add(ParseHTMLElement());
                        break;
                    default:
                        throw new NotImplementedException();
                }
                CurrentPosition++;
            }
        }
        private TextElement ParseHTMLElement()
        {
            if (!HTMLToken.IsBeginning)
                throw new NotImplementedException();

            switch (CurrentToken.GetKind())
            {
                case Token.Kind.Paragraph:
                    return ParseParagraph();
                case Token.Kind.Table:
                    return ParseTable();
                case Token.Kind.TableRow:
                    return ParseTableRow();
                case Token.Kind.TableDatum:
                    break;
                case Token.Kind.Header:
                    break;
                case Token.Kind.HorizontalRuling:
                    break;
                case Token.Kind.Span:
                    break;
                case Token.Kind.Link:
                    break;
                case Token.Kind.LineBreak:
                    break;
                default:
                    break;
            }
        }
        private Paragraph ParseParagraph()
        {

        }
        private Table ParseTable()
        {

        }
        private Table ParseTableRow()
        {

        }
        private Table ParseTableDatum()
        {

        }
        private Header ParseHeader()
        {

        }
    }
}
