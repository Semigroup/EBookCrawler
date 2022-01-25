using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBookCrawler.Parsing;

namespace EBookCrawler.Texting
{
   public class Transformer
    {
        public TextElement Transform(Parser.Node node)
        {
            IEnumerable<TextElement> children = node.Children.Select(child => Transform(child));

            if (node.IsRoot)
            {
                var container = new ContainerElement();
                container.Add(children);
                return container;
            }

            switch (node.Token.GetKind())
            {
                case Token.Kind.Raw:
                    break;
                case Token.Kind.Paragraph:
                    break;
                case Token.Kind.Table:
                    break;
                case Token.Kind.TableRow:
                    break;
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
                case Token.Kind.Bold:
                    break;
                case Token.Kind.Italic:
                    break;
                case Token.Kind.Emphasis:
                    break;
                case Token.Kind.TeleType:
                    break;
                case Token.Kind.Div:
                    break;
                case Token.Kind.Image:
                    break;
                case Token.Kind.Font:
                    break;
                case Token.Kind.Big:
                    break;
                case Token.Kind.Small:
                    break;
                case Token.Kind.Super:
                    break;
                case Token.Kind.Sub:
                    break;
                case Token.Kind.Verbatim:
                    break;
                case Token.Kind.BlockQuote:
                    break;
                case Token.Kind.List:
                    break;
                case Token.Kind.ListItem:
                    break;
                default:
                    break;
            }
        }
    }
}
