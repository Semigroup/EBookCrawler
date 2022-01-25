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
        private string Text;

        public TextElement Transform(string Text, Parser.Node node)
        {
            this.Text = Text;
            return Transform(node);
        }
        private TextElement Transform(Parser.Node node)
        {
            if (node.IsRoot)
            {
                var container = new ContainerElement();
                container.Add(TransformChildren(node.Children, true));
                return container;
            }

            switch (node.Token.MyKind)
            {
                case Token.Kind.Paragraph:
                case Token.Kind.Span:
                case Token.Kind.Link:
                case Token.Kind.Bold:
                case Token.Kind.Italic:
                case Token.Kind.Emphasis:
                case Token.Kind.TeleType:
                case Token.Kind.Div:
                case Token.Kind.Font:
                case Token.Kind.Big:
                case Token.Kind.Small:
                case Token.Kind.Super:
                case Token.Kind.Sub:
                case Token.Kind.BlockQuote:
                case Token.Kind.TableDatum:
                case Token.Kind.Header:
                    var container = GetContainer(node.Token);
                    container.Add(TransformChildren(node.Children, false));
                    return container;

                case Token.Kind.List:
                    var list = GetContainer(node.Token);
                    list.Add(TransformListItems(node.Children));
                    return list;

                case Token.Kind.Table:
                    var table = GetTable(node.Token);
                    table.Add(TransformRows(node.Children));
                    return table;
                case Token.Kind.TableRow:
                    var row = GetTableRow(node.Token);
                    row.Add(TransformTableData(node.Children));
                    return row;

                case Token.Kind.HorizontalRuling:
                    return TransformRule(node);
                case Token.Kind.LineBreak:
                    return TransformLinebreak(node);
                case Token.Kind.Image:
                    return TransformImage(node);
                case Token.Kind.Verbatim:
                    return TransformVerbatim(node);
                case Token.Kind.Raw:
                    var wordContainer = new ContainerElement();
                    wordContainer.Add(SplitRaw(node.Token.Text));
                    return wordContainer;
                default:
                    throw new NotImplementedException();
            }
        }
        private IEnumerable<TextElement> TransformChildren(IEnumerable<Parser.Node> children, bool ignoreRaw)
        {
            if (ignoreRaw)
            {
                foreach (var child in children)
                    if (!child.Token.IsRaw)
                        yield return Transform(child);
            }
            else
            {
                foreach (var child in children)
                    yield return Transform(child);
            }
        }
        private IEnumerable<TextElement> TransformRows(IEnumerable<Parser.Node> children)
        {
            foreach (var child in children)
                switch (child.Token.MyKind)
                {
                    case Token.Kind.Raw:
                        break;

                    case Token.Kind.TableRow:
                        yield return Transform(child);
                        break;

                    default:
                        throw new NotImplementedException();
                }
        }
        private IEnumerable<TextElement> TransformTableData(IEnumerable<Parser.Node> children)
        {
            foreach (var child in children)
                switch (child.Token.MyKind)
                {
                    case Token.Kind.Raw:
                        break;

                    case Token.Kind.TableDatum:
                        yield return Transform(child);
                        break;

                    default:
                        throw new NotImplementedException();
                }
        }

        private IEnumerable<TextElement> TransformListItems(IEnumerable<Parser.Node> children)
        {
            foreach (var child in children)
                switch (child.Token.MyKind)
                {
                    case Token.Kind.Raw:
                        break;

                    case Token.Kind.ListItem:
                        yield return Transform(child);
                        break;

                    default:
                        throw new NotImplementedException();
                }
        }

        private Verbatim TransformVerbatim(Parser.Node node)
        {
            var token = node.Token;
            int end = token.EndPosition;
            int start = token.Position + token.Length;
            return new Verbatim() { Text = Text.Substring(start, end - start) };
        }
        private Image TransformImage(Parser.Node node)
        {
            var token = node.Token;
            Image img = new Image();
            foreach (var att in token.Attributes)
                switch (att.Name.ToLower())
                {
                    case "alt":
                        img.Name = att.Value;
                        break;
                    case "height":
                        img.Height = att.ValueAsDouble();
                        break;
                    case "width":
                        img.Width = att.ValueAsDouble();
                        break;
                    case "vspace":
                        img.VSpace = att.ValueAsDouble();
                        break;
                    case "hspace":
                        img.HSpace = att.ValueAsDouble();
                        break;
                    case "src":
                        img.RelativePath = att.Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return img;
        }
        private WhiteSpace TransformLinebreak(Parser.Node node)
        {
            return new WhiteSpace() { VSpace = 1 };
        }
        private HorizontalRule TransformRule(Parser.Node node)
        {
            var token = node.Token;
            var hr = new HorizontalRule();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "class":
                        switch (attribute.Value)
                        {
                            case "short":
                                hr.Length = 0.3;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return hr;
        }

        private ContainerElement GetContainer(Token token)
        {
            switch (token.MyKind)
            {
                case Token.Kind.Paragraph:
                    return GetParagraph(token);

                case Token.Kind.Bold:
                case Token.Kind.Italic:
                case Token.Kind.Emphasis:
                case Token.Kind.TeleType:
                case Token.Kind.Font:
                case Token.Kind.Big:
                case Token.Kind.Small:
                    return GetStyleContainer(token);

                case Token.Kind.TableDatum:
                    return GetTableDatum(token);

                case Token.Kind.Span:
                    return GetSpanContainer(token);
                case Token.Kind.Link:
                    return GetLink(token);
                case Token.Kind.Div:
                    return GetDiv(token);
                case Token.Kind.Super:
                case Token.Kind.Sub:
                    return GetSuperSub(token);
                case Token.Kind.BlockQuote:
                    return GetBlockQuote(token);
                case Token.Kind.List:
                    return GetList(token);
                case Token.Kind.ListItem:
                    return GetListItem(token);
                default:
                    throw new NotImplementedException();
            }
        }
        private ContainerElement GetParagraph(Token token)
        {
            if (token.GetAttribute("class") == "vers")
            {
                var poem = new Poem();
                foreach (var attribute in token.Attributes)
                    switch (attribute.Name.ToLower())
                    {
                        case "class":
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                return poem;
            }
            else
            {
                var para = new Paragraph();
                foreach (var attribute in token.Attributes)
                    switch (attribute.Name.ToLower())
                    {
                        case "class":
                            para.SetClass(attribute.Value);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                return para;
            }
        }
        private ContainerElement GetStyleContainer(Token token)
        {
            var container = new ContainerElement();
            switch (token.MyKind)
            {
                case Token.Kind.Bold:
                    container.Style = new Style() { IsBold = true };
                    break;
                case Token.Kind.Italic:
                    container.Style = new Style() { IsItalic = true };
                    break;
                case Token.Kind.Emphasis:
                    container.Style = new Style() { IsEmphasis = true };
                    break;
                case Token.Kind.TeleType:
                    container.Style = new Style() { IsMonoSpace = true };
                    break;
                case Token.Kind.Font:
                    foreach (var attribute in token.Attributes)
                        switch (attribute.Name.ToLower())
                        {
                            case "size":
                                container.Size = (int)attribute.ValueAsDouble();
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    break;
                case Token.Kind.Big:
                    container.Size = 1;
                    break;
                case Token.Kind.Small:
                    container.Size = -1;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return container;
        }
        private ContainerElement GetSpanContainer(Token token)
        {
            var container = new ContainerElement();
            switch (token.GetAttribute("class"))
            {
                case "speaker":
                    container.Style = new Style() { IsBold = true };
                    break;
                case "regie":
                    container.Style = new Style() { IsItalic = true };
                    break;
                case "lower":
                    container.Style = new Style() { IsLower = true };
                    break;
                case "upper":
                    container.Style = new Style() { IsUpper = true };
                    break;
                case "smallcaps":
                    container.Style = new Style() { IsSmallCaps = true };
                    break;
                case "lektorat":
                    container.Color = new Color("a9a9a9");
                    break;
                case "fotnote":
                case "footnote":
                    container = new Footnote();
                    break;
                case "wide":
                    container.Style = new Style() { IsWide = true };
                    break;
                case "tooltip":
                    container = new Footnote() { IsToolTip = true };
                    break;
                default:
                    throw new NotImplementedException();
            }

            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "class":
                        break;
                    case "style":
                        container.Color = new Color(attribute.Value);
                        break;
                    case "title":
                        if ((container as Footnote).IsToolTip)
                            container.Add(SplitRaw(attribute.Value));
                        else
                            throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return container;
        }
        private ContainerElement GetLink(Token token)
        {
            Link link = new Link();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "name":
                        link.Name = attribute.Value;
                        break;
                    case "href":
                        link.URL = attribute.Value;
                        break;
                    case "id":
                        link.ID = attribute.Value;
                        break;
                    case "title":
                        link.Title = attribute.Value;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            return link;
        }
        private ContainerElement GetDiv(Token token)
        {
            var cont = new ContainerElement();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "class":
                        if (attribute.Value.ToLower() == "toc")
                            return new EmptyContainer();
                        else
                            cont.SetClass(attribute.Value);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return cont;
        }
        private ContainerElement GetSuperSub(Token token)
        {
            ContainerElement cont;
            switch (token.MyKind)
            {
                case Token.Kind.Super:
                    cont = new SuperIndex();
                    break;
                case Token.Kind.Sub:
                    cont = new SubIndex();
                    break;
                default:
                    throw new NotImplementedException();
            }
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    default:
                        throw new NotImplementedException();
                }
            return cont;
        }
        private ContainerElement GetBlockQuote(Token token)
        {
            BlockQuote bq = new BlockQuote();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    default:
                        throw new NotImplementedException();
                }
            return bq;
        }
        private ContainerElement GetListItem(Token token)
        {
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    default:
                        throw new NotImplementedException();
                }
            return new ContainerElement();
        }
        private ContainerElement GetList(Token token)
        {
            TextList list = new TextList();
            switch (token.Tag)
            {
                case "ol":
                    list.IsOrdered = true;
                    break;
                case "ul":
                    list.IsOrdered = false;
                    break;
                default:
                    throw new NotImplementedException();
            }

            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    default:
                        throw new NotImplementedException();
                }
            return list;
        }
        private ContainerElement GetHeader(Token token)
        {
            Header header = new Header
            {
                Hierarchy = int.Parse(token.Tag.Substring(1))
            };

            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    default:
                        throw new NotImplementedException();
                }
            return header;
        }

        private Table GetTable(Token token)
        {
            Table table = new Table();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "align":
                        table.SetAlignment(attribute.Value);
                        break;
                    case "border":
                        table.Border = attribute.ValueAsDouble();
                        break;
                    case "padding":
                    case "cellpadding":
                        table.Padding = attribute.ValueAsDouble();
                        break;
                    case "spacing":
                    case "cellspacing":
                        table.Spacing = attribute.ValueAsDouble();
                        break;
                    //case "class":
                    //    table.Class = attribute.Value;
                    //    break;
                    case "summary":
                        table.Caption = attribute.Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return table;
        }
        private Table.Row GetTableRow(Token token)
        {
            Table.Row table = new Table.Row();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    default:
                        throw new NotImplementedException();
                }
            return table;
        }
        private Table.Datum GetTableDatum(Token token)
        {
            Table.Datum datum = new Table.Datum();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "class":
                        datum.SetClass(attribute.Value);
                        break;
                    case "align":
                        datum.SetClass(attribute.Value);
                        break;
                    case "valign":
                        datum.SetVAlignment(attribute.Value);
                        break;
                    case "colspan":
                        datum.ColSpan = (int)attribute.ValueAsDouble();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return datum;
        }

        private IEnumerable<TextElement> SplitRaw(string rawText)
        {
            var words = rawText.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in words)
            {
                yield return new Word() { Value = word };
                yield return new WhiteSpace() { HSpace = 1 };
            }
        }
    }
}
