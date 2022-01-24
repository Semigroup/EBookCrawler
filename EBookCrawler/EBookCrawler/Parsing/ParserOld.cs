using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBookCrawler.Texting;

namespace EBookCrawler.Parsing
{
    public class ParserOld
    {
        //public string Text { get; set; }
        //public Token[] Tokens { get; set; }
        //public int CurrentPosition { get; set; }
        //public Token CurrentToken => Tokens[CurrentPosition];
        //public HTMLToken HTMLToken => CurrentToken as HTMLToken;

        //public FontManager FontManager { get; set; }

        //public Document ParseDocument(IEnumerable<Token> tokens, string text)
        //{
        //    this.Text = text;
        //    this.FontManager = new FontManager();
        //    this.Tokens = tokens.ToArray();
        //    Document doc = new Document();

        //    while (CurrentPosition < Tokens.Length)
        //    {
        //        switch (CurrentToken.GetKind())
        //        {
        //            case Token.Kind.Raw:
        //                CurrentPosition++;
        //                break;
        //            case Token.Kind.Div:
        //            case Token.Kind.Link:
        //            case Token.Kind.Paragraph:
        //            case Token.Kind.Table:
        //            case Token.Kind.Header:
        //            case Token.Kind.HorizontalRuling:
        //            case Token.Kind.Verbatim:
        //            case Token.Kind.BlockQuote:
        //            case Token.Kind.List:
        //                doc.Add(ParseStartTag());
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    return doc;
        //}
        //private void Skip()
        //{
        //    int end = FindEnd();
        //    CurrentPosition = end + 1;
        //}
        //private IEnumerable<TextElement> ParseToken()
        //{
        //    if (CurrentToken.IsRaw)
        //        return ParseRawToken();
        //    else
        //        return ParseStartTag();
        //}
        //private IEnumerable<TextElement> ParseStartTag()
        //{
        //    if (!HTMLToken.IsBeginning)
        //        throw new NotImplementedException();

        //    switch (CurrentToken.GetKind())
        //    {
        //        case Token.Kind.Paragraph:
        //            return ParseParagraph();
        //        case Token.Kind.Table:
        //            return ParseTable();
        //        case Token.Kind.TableRow:
        //            return ParseTableRow();
        //        case Token.Kind.TableDatum:
        //            return ParseTableDatum();
        //        case Token.Kind.Header:
        //            return ParseHeader();
        //        case Token.Kind.HorizontalRuling:
        //            return ParseHorizontalRule();
        //        case Token.Kind.Link:
        //            return ParseLink();
        //        case Token.Kind.LineBreak:
        //            return ParseLineBreak();
        //        case Token.Kind.Div:
        //            return ParseDiv();
        //        case Token.Kind.Span:
        //        case Token.Kind.Bold:
        //        case Token.Kind.Italic:
        //        case Token.Kind.TeleType:
        //        case Token.Kind.Emphasis:
        //        case Token.Kind.Sub:
        //        case Token.Kind.Super:
        //        case Token.Kind.Small:
        //        case Token.Kind.Big:
        //            return ParseFontMode();
        //        case Token.Kind.Image:
        //            return ParseImage();
        //        case Token.Kind.Verbatim:
        //            return ParseVerbatim();
        //        case Token.Kind.BlockQuote:
        //            return ParseBlockQuote();
        //        case Token.Kind.List:
        //            return ParseList();
        //        case Token.Kind.ListItem:
        //            return ParseListItem();
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}
        //private IEnumerable<TextElement> ParseFontMode()
        //{
        //    if (HTMLToken.IsBeginning && HTMLToken.IsEnd)
        //        yield break;
        //    if (HTMLToken.IsBeginning)
        //    {
        //        switch (CurrentToken.GetKind())
        //        {
        //            case Token.Kind.Bold:
        //                this.FontManager.AddBold();
        //                break;
        //            case Token.Kind.Italic:
        //                this.FontManager.AddItalic();
        //                break;
        //            case Token.Kind.TeleType:
        //                this.FontManager.AddTeleType();
        //                break;
        //            case Token.Kind.Emphasis:
        //                this.FontManager.AddEmphasis();
        //                break;
        //            case Token.Kind.Super:
        //                this.FontManager.AddSuper();
        //                break;
        //            case Token.Kind.Sub:
        //                this.FontManager.AddSub();
        //                break;
        //            case Token.Kind.Big:
        //                this.FontManager.AddBig();
        //                break;
        //            case Token.Kind.Small:
        //                this.FontManager.AddSmall();
        //                break;
        //            case Token.Kind.Span:
        //                FontManager.Mode delta = new FontManager.Mode();
        //                bool isTooltip = false;
        //                foreach (var attribute in HTMLToken.Attributes)
        //                    switch (attribute.Name.ToLower())
        //                    {
        //                        case "class":
        //                            switch (attribute.Value.ToLower())
        //                            {
        //                                case "speaker":
        //                                    delta.IsBold = true;
        //                                    break;
        //                                case "regie":
        //                                    delta.IsItalic = true;
        //                                    break;
        //                                case "lower":
        //                                    delta.IsLower = true;
        //                                    break;
        //                                case "upper":
        //                                    delta.IsUpper = true;
        //                                    break;
        //                                case "smallcaps":
        //                                    delta.IsSmallCaps = true;
        //                                    break;
        //                                case "lektorat":
        //                                    delta.ChangeColor = true;
        //                                    delta.SetColor("a9a9a9");
        //                                    break;
        //                                case "fotnote":
        //                                case "footnote":
        //                                    delta.IsFootnote = true;
        //                                    break;
        //                                case "wide":
        //                                    delta.IsWide = true;
        //                                    break;
        //                                case "tooltip":
        //                                    isTooltip = true;
        //                                    break;
        //                                default:
        //                                    throw new NotImplementedException();
        //                            }
        //                            break;
        //                        case "style":
        //                            delta.ChangeColor = true;
        //                            delta.SetColor(attribute.Value);
        //                            break;
        //                        case "title":
        //                            if (isTooltip)
        //                                delta.ToolTip = attribute.Value;
        //                            else
        //                                throw new NotImplementedException();
        //                            break;
        //                        default:
        //                            throw new NotImplementedException();
        //                    }
        //                FontManager.Add("span", delta);
        //                break;
        //            case Token.Kind.Font:
        //                FontManager.Mode deltaFont = new FontManager.Mode();
        //                foreach (var attribute in HTMLToken.Attributes)
        //                    switch (attribute.Name.ToLower())
        //                    {
        //                        case "size":
        //                            deltaFont.Size = (int)attribute.ValueAsDouble();
        //                            break;
        //                        default:
        //                            throw new NotImplementedException();
        //                    }
        //                FontManager.Add("font", deltaFont);
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    if (HTMLToken.IsEnd)
        //    {
        //        var removed = FontManager.Remove(HTMLToken.Tag);
        //        if (removed.ToolTip != null)
        //        {
        //            CurrentPosition++;
        //            yield return new Footnote(removed.ToolTip);
        //        }
        //    }

        //    CurrentPosition++;
        //    yield break;
        //}
        //private int FindEnd()
        //{
        //    if (HTMLToken.IsEnd)
        //        return CurrentPosition;
        //    else if (CurrentPosition == Tokens.Length - 1)
        //        throw new NotImplementedException();

        //    string tag = HTMLToken.Tag;

        //    int endPos = CurrentPosition + 1;
        //    while (endPos < Tokens.Length)
        //    {
        //        var curTok = Tokens[endPos];
        //        if (curTok is HTMLToken tok && tok.Tag == tag)
        //            if (tok.IsBeginning)
        //                throw new NotImplementedException();
        //            else
        //                return endPos;
        //        endPos++;
        //    }
        //    throw new NotImplementedException();
        //}
        //private IEnumerable<TextElement> ParseRawToken()
        //{
        //    RawToken token = CurrentToken as RawToken;
        //    CurrentPosition++;

        //    return SplitRaw(token.Text, FontManager.CurrentMode);
        //}
        //public static IEnumerable<TextElement> SplitRaw(string raw, FontManager.Mode mode)
        //{
        //    bool first = true;
        //    foreach (var word in raw.Split(' ', '\n', '\r'))
        //    {
        //        if (first)
        //            first = false;
        //        else
        //            yield return new WhiteSpace() { HSpace = 1, Mode = mode };
        //        yield return new Word() { Value = word, Mode = mode };
        //    }
        //}
        //private IEnumerable<TextElement> ParseLineBreak()
        //{
        //    CurrentPosition++;
        //    yield return new WhiteSpace() { VSpace = 1 };
        //}
        //private IEnumerable<TextElement> ParseParagraph()
        //{
        //    Paragraph para = new Paragraph();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            case "class":
        //                para.Class = attribute.Value;
        //                para.SetAlignment(attribute.Value);
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    FillTextContainer(para);

        //    yield return para;
        //}
        //private IEnumerable<TextElement> ParseBlockQuote()
        //{
        //    BlockQuote bq = new BlockQuote();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    FillTextContainer(bq);

        //    yield return bq;
        //}
        //private void FillTextContainer<T>(T container) where T : ContainerElement
        //{
        //    var end = FindEnd();
        //    CurrentPosition++;
        //    while (CurrentPosition < end)
        //    {
        //        switch (CurrentToken.GetKind())
        //        {
        //            case Token.Kind.Paragraph:
        //                container.Add(ParseParagraph());
        //                break;
        //            case Token.Kind.Raw:
        //                container.Add(ParseRawToken());
        //                break;
        //            case Token.Kind.LineBreak:
        //                if (HTMLToken.IsBeginning)
        //                    container.Add(ParseStartTag());
        //                else
        //                    CurrentPosition++;
        //                break;
        //            case Token.Kind.Link:
        //                container.Add(ParseStartTag());
        //                break;
        //            case Token.Kind.Image:
        //                container.Add(ParseImage());
        //                break;
        //            case Token.Kind.HorizontalRuling:
        //                container.Add(ParseHorizontalRule());
        //                break;
        //            case Token.Kind.Font:
        //            case Token.Kind.Span:
        //            case Token.Kind.TeleType:
        //            case Token.Kind.Bold:
        //            case Token.Kind.Italic:
        //            case Token.Kind.Emphasis:
        //            case Token.Kind.Sub:
        //            case Token.Kind.Super:
        //            case Token.Kind.Small:
        //            case Token.Kind.Big:
        //                container.Add(ParseFontMode());
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    if (CurrentPosition == end)
        //        CurrentPosition++;
        //}
        //private IEnumerable<TextElement> ParseTable()
        //{
        //    Table table = new Table();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            case "align":
        //                table.SetAlignment(attribute.Value);
        //                break;
        //            case "border":
        //                table.Border = attribute.ValueAsDouble();
        //                break;
        //            case "padding":
        //            case "cellpadding":
        //                table.Padding = attribute.ValueAsDouble();
        //                break;
        //            case "spacing":
        //            case "cellspacing":
        //                table.Spacing = attribute.ValueAsDouble();
        //                break;
        //            case "class":
        //                table.Class = attribute.Value;
        //                break;
        //            case "summary":
        //                table.Summary = attribute.Value;
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }

        //    var end = FindEnd();
        //    CurrentPosition++;
        //    while (CurrentPosition < end)
        //    {
        //        switch (CurrentToken.GetKind())
        //        {
        //            case Token.Kind.Raw:
        //                CurrentPosition++;
        //                break;
        //            case Token.Kind.TableRow:
        //                table.Add(ParseTableRow());
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    if (CurrentPosition == end)
        //        CurrentPosition++;
        //    yield return table;
        //}
        //private IEnumerable<TextElement> ParseTableRow()
        //{
        //    Table.Row table = new Table.Row();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            default:
        //                throw new NotImplementedException();
        //        }

        //    var end = FindEnd();
        //    CurrentPosition++;
        //    while (CurrentPosition < end)
        //    {
        //        switch (CurrentToken.GetKind())
        //        {
        //            case Token.Kind.Raw:
        //                CurrentPosition++;
        //                break;
        //            case Token.Kind.TableDatum:
        //                table.Add(ParseTableDatum());
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    }
        //    if (CurrentPosition == end)
        //        CurrentPosition++;
        //    yield return table;
        //}
        //private IEnumerable<TextElement> ParseTableDatum()
        //{
        //    Table.Datum datum = new Table.Datum();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            case "class":
        //                datum.Class = attribute.Value;
        //                break;
        //            case "align":
        //                datum.SetAlignment(attribute.Value);
        //                break;
        //            case "valign":
        //                datum.SetVAlignment(attribute.Value);
        //                break;
        //            case "colspan":
        //                datum.ColSpan = (int)attribute.ValueAsDouble();
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    FillTextContainer(datum);

        //    yield return datum;
        //}
        //private IEnumerable<TextElement> ParseHeader()
        //{
        //    Header header = new Header() { Hierarchy = int.Parse(HTMLToken.Tag.Substring(1)) };
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            case "class":
        //                header.Class = attribute.Value;
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    FillTextContainer(header);

        //    yield return header;
        //}
        //private IEnumerable<TextElement> ParseHorizontalRule()
        //{
        //    HorizontalRule hr = new HorizontalRule();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            case "class":
        //                hr.Class = attribute.Value;
        //                switch (attribute.Value)
        //                {
        //                    case "short":
        //                        hr.Length = 0.3;
        //                        break;
        //                    default:
        //                        throw new NotImplementedException();
        //                }
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    CurrentPosition++;
        //    yield return hr;
        //}
        //private IEnumerable<TextElement> ParseLink()
        //{
        //    Link link = new Link();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            case "name":
        //                link.Name = attribute.Value;
        //                break;
        //            case "href":
        //                link.URL = attribute.Value;
        //                break;
        //            case "id":
        //                link.ID = attribute.Value;
        //                break;
        //            case "title":
        //                link.Title = attribute.Value;
        //                break;

        //            default:
        //                throw new NotImplementedException();
        //        }
        //    FillTextContainer(link);

        //    yield return link;
        //}
        //private IEnumerable<TextElement> ParseDiv()
        //{
        //    Div div = new Div();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            case "class":
        //                if (attribute.Value.ToLower() == "toc")
        //                {
        //                    Skip();
        //                    yield break;
        //                }
        //                else
        //                {
        //                    div.Class = attribute.Value;
        //                    div.SetAlignment(attribute.Value);
        //                }
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    FillTextContainer(div);

        //    yield return div;
        //}
        //private IEnumerable<TextElement> ParseImage()
        //{
        //    Image img = new Image();
        //    foreach (var att in HTMLToken.Attributes)
        //        switch (att.Name.ToLower())
        //        {
        //            case "alt":
        //                img.Name = att.Value;
        //                break;
        //            case "height":
        //                img.Height = att.ValueAsDouble();
        //                break;
        //            case "width":
        //                img.Width = att.ValueAsDouble();
        //                break;
        //            case "vspace":
        //                img.VSpace = att.ValueAsDouble();
        //                break;
        //            case "hspace":
        //                img.HSpace = att.ValueAsDouble();
        //                break;
        //            case "src":
        //                img.RelativePath = att.Value;
        //                break;
        //            default:
        //                throw new NotImplementedException();
        //        }

        //    CurrentPosition++;
        //    yield return img;
        //}
        //private IEnumerable<TextElement> ParseVerbatim()
        //{
        //    int end = FindEnd();
        //    int a = HTMLToken.Position + HTMLToken.Length;
        //    int b = (Tokens[end] as HTMLToken).Position;

        //    CurrentPosition = end + 1;

        //    yield return new Verbatim() { Text = Text.Substring(a, b - a) };
        //}
        //private IEnumerable<TextElement> ParseList()
        //{
        //    TextList list = new TextList();
        //    switch (HTMLToken.Tag)
        //    {
        //        case "ol":
        //            list.IsOrdered = true;
        //            break;
        //        case "ul":
        //            list.IsOrdered = false;
        //            break;
        //        default:
        //            throw new NotImplementedException();
        //    }

        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    int end = FindEnd();
        //    CurrentPosition++;
        //    while (CurrentPosition < end)
        //    {
        //        switch (CurrentToken.GetKind())
        //        {
        //            case Token.Kind.Raw:
        //                CurrentPosition++;
        //                break;
        //            case Token.Kind.ListItem:
        //                list.Add(ParseListItem());
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    if (CurrentPosition == end)
        //        CurrentPosition++;
        //    yield return list;
        //}
        //private IEnumerable<TextElement> ParseListItem()
        //{
        //    TextList.Item item = new TextList.Item();
        //    foreach (var attribute in HTMLToken.Attributes)
        //        switch (attribute.Name.ToLower())
        //        {
        //            default:
        //                throw new NotImplementedException();
        //        }
        //    FillTextContainer(item);

        //    yield return item;
        //}
    }
}
