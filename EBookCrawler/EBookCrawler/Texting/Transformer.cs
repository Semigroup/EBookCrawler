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

        public TextChapter Transform(string Text, Parser.Node node)
        {
            this.Text = Text;
            ComputeStyle(node);
            return Transform(node) as TextChapter;
        }
        private void ComputeStyle(Parser.Node node)
        {
            if (!node.IsRoot)
                node.Token.ComputeStyle();
            if (!node.IsLeaf)
                foreach (var child in node.Children)
                    ComputeStyle(child);
        }
        private TextElement Transform(Parser.Node node)
        {
            if (node.IsRoot)
            {
                var chapter = new TextChapter();
                chapter.Add(TransformChildren(node.Children, true));
                return chapter;
            }

            switch (node.Token.MyKind)
            {
                case Token.Kind.List:
                    var list = GetContainer(node.Token);
                    list.Add(TransformChildren(node.Children, false));
                    return list;

                case Token.Kind.Table:
                    var table = GetTable(node.Token);
                    table.Add(TransformRows(node.Children));
                    return table;
                case Token.Kind.TableRow:
                    var row = GetTableRow(node.Token);
                    row.Add(TransformTableData(node.Children));
                    return row;
                case Token.Kind.TableHead:
                case Token.Kind.TableBody:
                case Token.Kind.TableFoot:
                    var head = GetTableRowContainer(node.Token);
                    head.Add(TransformRows(node.Children));
                    return head;

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
                    var container = GetContainer(node.Token);
                    container.Add(TransformChildren(node.Children, false));
                    return container;
            }
        }
        private IEnumerable<TextElement> TransformChildren(IEnumerable<Parser.Node> children, bool ignoreRaw)
        {
            if (children == null)
                yield break;

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

                    case Token.Kind.ColumnGroup:
                        //ToDo?
                        break;

                    case Token.Kind.TableHead:
                    case Token.Kind.TableBody:
                    case Token.Kind.TableFoot:
                    case Token.Kind.TableRow:
                    case Token.Kind.Caption:
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

        //private IEnumerable<TextElement> TransformListItems(IEnumerable<Parser.Node> children)
        //{
        //    foreach (var child in children)
        //        switch (child.Token.MyKind)
        //        {
        //            case Token.Kind.Raw:
        //                break;

        //            case Token.Kind.ListItem:
        //                yield return Transform(child);
        //                break;
        //            case Token.Kind.ListTerm:
        //                yield return Transform(child);
        //                break;

        //            default:
        //                throw new NotImplementedException();
        //        }
        //}

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
                    case "altalt":
                        img.AlternativeTitle = att.Value;
                        break;
                    case "name":
                    case "title":
                        img.Title = att.Value;
                        break;
                    case "height":
                        img.Height = att.ValueAsLength();
                        break;
                    case "width":
                        img.Width = att.ValueAsLength();
                        break;
                    case "vspace":
                    case "vpsace":
                        img.VSpace = att.ValueAsDouble();
                        break;
                    case "hspace":
                        img.HSpace = att.ValueAsDouble();
                        break;
                    case "src":
                        img.RelativePath = att.Value;
                        break;
                    case "class":
                        switch (att.Value.ToLower())
                        {
                            case "initial":
                            case "intial":
                            case "fbu":
                            case "half":
                                img.IsCapital = true;
                                break;
                            case "right":
                            case "center":
                            case "left":
                                img.WrapFigure = true;
                                img.Alignment = GetAlignment(att.Value);
                                break;
                            case "figure":
                            case "full":
                            case "autpic":
                                //ToDo?
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case "align":
                        img.Alignment = GetAlignment(att.Value);
                        switch (att.Value.ToLower())
                        {
                            case "bottom":
                            case "middle":
                            case "top":
                                img.InLine = true;
                                break;
                            default:
                                break;
                        }
                        break;
                    case "border":
                        img.Border = att.ValueAsDouble();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    case "display":
                        switch (prop.Value)
                        {
                            case "block":
                                //ToDo?
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case "margin":
                        switch (prop.Value)
                        {
                            case "auto":
                                //ToDo?
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return img;
        }
        private WhiteSpace TransformLinebreak(Parser.Node node)
        {
            return new WhiteSpace() { VSpace = 1, Indentation = 1 };
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
                                hr.Length = new Length() { Value = 0.3, IsProportional = true };
                                break;
                            case "":
                            case "center":
                                break;
                            case "star":
                                //ToDo
                                break;
                            case "empty":
                                hr.Length = new Length();
                                break;
                            case "thin":
                                //ToDo
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case "width":
                        hr.Length = attribute.ValueAsLength();
                        break;
                    case "id":
                    case "size":
                        break;
                    case "align":
                        hr.Alignment = GetAlignment(attribute.Value.ToLower());
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    default:
                        throw new NotImplementedException();
                }
            return hr;
        }

        private ContainerElement GetContainer(Token token)
        {
            var clazz = token.GetAttribute("class");
            if (clazz != null && clazz.ToLower() == "toc")
                return new EmptyContainer();
            switch (token.MyKind)
            {
                case Token.Kind.Div:
                    return GetDiv(token);
                case Token.Kind.Paragraph:
                    return GetParagraph(token);
                case Token.Kind.TableDatum:
                    return GetTableDatum(token);
                case Token.Kind.Caption:
                    return GetCaption(token);
                case Token.Kind.Span:
                    return GetSpanContainer(token);
                case Token.Kind.Link:
                    return GetLink(token);
                case Token.Kind.Super:
                case Token.Kind.Sub:
                    return GetSuperSub(token);
                case Token.Kind.Quote:
                case Token.Kind.BlockQuote:
                    return GetQuote(token);
                case Token.Kind.List:
                    return GetList(token);
                case Token.Kind.ListItem:
                case Token.Kind.ListTerm:
                    return GetListItem(token);
                case Token.Kind.Header:
                    return GetHeader(token);

                case Token.Kind.Aside:
                    //ToDo
                    return new ContainerElement();

                case Token.Kind.Column:
                case Token.Kind.ColumnGroup:
                    //ToDo
                    return new EmptyContainer();

                default:
                    return GetStyleContainer(token);
            }
        }
        private ContainerElement GetBoxOrPoem(Token token)
        {
            var clazz = token.GetAttribute("class");
            if (clazz != null)
                clazz = clazz.ToLower();
            else
            {
                clazz = token.GetAttribute("lass");
                if (clazz != null)
                    clazz = clazz.ToLower();
            }
            if (clazz == "ver" || clazz == "vers" || clazz == "poem")
            {
                var poem = new Poem();
                foreach (var attribute in token.Attributes)
                    switch (attribute.Name.ToLower())
                    {
                        case "lang":
                        case "class":
                        case "id":
                            break;
                        case "align":
                            poem.Alignment = GetAlignment(attribute.Value);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                return poem;
            }
            else if (clazz == "box" || clazz == "kasten")
            {
                var box = new Box();
                foreach (var attribute in token.Attributes)
                    switch (attribute.Name.ToLower())
                    {
                        case "class":
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                return box;
            }
            else if (clazz == "lektorat")
                return new EmptyContainer();
            else
                return null;
        }
        private ContainerElement GetParagraph(Token token)
        {
            var bop = GetBoxOrPoem(token);
            if (bop != null)
                return bop;
            else
            {
                var para = new Paragraph();
                if (token.Tag == "center")
                    para.Alignment = 1;
                foreach (var attribute in token.Attributes)
                    switch (attribute.Name.ToLower())
                    {
                        case "align":
                        case "lass":
                        case "class":
                            para.SetClass(attribute.Value.ToLower());
                            break;
                        case "id":
                        case "lang"://Language
                        case "summary":
                        case "name":
                        case "title":
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                foreach (var prop in token.StyleProperties)
                    switch (prop.Name)
                    {
                        case "margin-left":
                            para.LeftMargin = prop.ValueAsMeasure();
                            break;
                        case "text-indent":
                            //ToDo?
                            //Add Latex command here?
                            para.StartsWithIndentation = true;
                            break;
                        case "text-align":
                            para.Alignment = GetAlignment(prop.Value);
                            break;
                        case "font-variant":
                        case "font-weight":
                        case "font-style":
                            para.Style = GetFontStyle(prop.Value);
                            break;
                        case "font-size":
                            para.Size = GetSize(prop.Value);
                            break;
                        case "margin-top":
                        case "margin-bottom":
                        case "page-break-before":
                        case "page-break-after":
                            //ToDo
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                return para;
            }
        }
        private ContainerElement GetDiv(Token token)
        {
            var bop = GetBoxOrPoem(token);
            if (bop != null)
                return bop;
            else
            {
                var div = new Div();
                foreach (var attribute in token.Attributes)
                    switch (attribute.Name.ToLower())
                    {
                        case "class":
                            div.SetKind(attribute.Value.ToLower());
                            break;
                        case "align":
                            div.Alignment = GetAlignment(attribute.Value.ToLower());
                            break;
                        case "lang":
                        case "id":
                        case "title":
                            break;
                        case "name":
                            div.SetKind(attribute.Value.ToLower());
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                foreach (var prop in token.StyleProperties)
                    switch (prop.Name)
                    {
                        default:
                            throw new NotImplementedException();
                    }
                return div;
            }
        }
        private ContainerElement GetStyleContainer(Token token)
        {
            var container = new ContainerElement();
            switch (token.MyKind)
            {
                case Token.Kind.Strike:
                    container.Style = new Style() { IsCrossedOut = true };
                    break;
                case Token.Kind.Bold:
                    container.Style = new Style() { IsBold = true };
                    break;
                case Token.Kind.Italic:
                case Token.Kind.Address:
                case Token.Kind.Cite:
                    container.Style = new Style() { IsItalic = true };
                    break;
                case Token.Kind.Emphasis:
                    container.Style = new Style() { IsEmphasis = true };
                    break;
                case Token.Kind.TeleType:
                    container.Style = new Style() { IsMonoSpace = true };
                    break;
                case Token.Kind.Underlined:
                    container.Style = new Style() { IsUnderlined = true };
                    break;
                case Token.Kind.Insertion:
                    container.Style = new Style() { IsUnderlined = true };
                    container.Color = new Color("0000ff");
                    break;
                case Token.Kind.Deletion:
                    container.Style = new Style() { IsCrossedOut = true };
                    container.Color = new Color("ff0000");
                    break;
                case Token.Kind.Font:
                    foreach (var attribute in token.Attributes)
                        switch (attribute.Name.ToLower())
                        {
                            case "size":
                                container.Size += (int)attribute.ValueAsDouble();
                                break;
                            case "color":
                                container.Color = new Color(attribute.Value);
                                break;
                            case "class":
                                break;

                            case "face":
                                //ToDo?
                                //Times New Roman
                                break;
                            default:
                                throw new NotImplementedException();
                        }

                    foreach (var prop in token.StyleProperties)
                        switch (prop.Name)
                        {
                            case "font-size":
                                //ToDo?
                                break;
                            case "margin-left":
                                container.LeftMargin = prop.ValueAsMeasure();
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
            var clazz = token.GetAttribute("class");
            if (clazz == null)
                return container;
            switch (clazz.ToLower())
            {
                case "speaker":
                case "rspeaker":
                    container.Style = new Style() { IsBold = true };
                    break;
                case "action":
                case "regie":
                case "direction":
                    container.Style = new Style() { IsItalic = true };
                    break;

                case "letorat":
                case "lektorat":
                    //container.Color = new Color("a9a9a9");
                    //break; ToDo?
                    return new EmptyContainer();

                case "titel0":
                    container.Size = 3;
                    container.Style = new Style() { IsUpper = true, IsWide = true };
                    break;
                case "titel1":
                case "titel":
                    container.Size = 1;
                    container.Style = new Style() { IsUpper = true, IsWide = true };
                    break;
                case "titel2":
                case "titel4":
                case "titel5":
                    container.Style = new Style() { IsUpper = true };
                    break;
                case "titel2a":
                    container.Style = new Style() { IsUpper = true, IsWide = true };
                    break;
                case "titel3":
                    container.Size = 1;
                    container.Style = new Style() { IsUpper = true };
                    break;

                case "footnote":
                case "fotnote":
                case "foonote":
                case "footnnote":
                case "footntote":
                    container = new Footnote();
                    break;
                case "tooltip":
                    container = new Footnote() { IsToolTip = true };
                    break;
                case "sidenote":
                    container = new Footnote() { IsSideNote = true };
                    break;

                case "big":
                case "big1":
                case "big2":
                case "big3":
                case "f120":
                case "f130":
                    container.Size += 1;
                    break;
                case "initial":
                case "riesig":
                case "big140":
                case "big155":
                case "f140":
                case "f150":
                case "size150":
                case "headline":
                case "firstline":
                    container.Size += 2;
                    break;
                case "ls":
                case "zweizeilig":
                    container.Size += 3;
                    break;
                case "big250":
                case "bigbracket":
                case "klammer":
                    container.Size += 5;
                    break;
                case "big580":
                    container.Size += 10;
                    break;
                case "anmerk":
                case "note":
                case "smaller":
                case "xsmall":
                case "small":
                case "stage":
                case "f100":
                case "f090":
                case "line":
                    container.Size -= 1;
                    break;
                case "font110":
                case "big110":
                case "big105":
                case "f110":
                case "f105":
                case "fntext":
                case "src":
                case "versnum":
                    break;

                case "center":
                    container.Alignment = 1;
                    break;
                case "word":
                case "wort":
                case "red":
                    container.Color = new Color("ff0000");
                    break;
                case "diff":
                case "green":
                    container.Color = new Color("00ff00");
                    break;
                case "c1781":
                case "je":
                    container.Color = new Color("0000ff");
                    break;

                case "kurz":
                case "unicode":
                case "titlecolor":
                case "authorh":
                case "title":
                case "latin":
                    break;

                case "block":
                case "block1":
                case "block2":
                case "block3":
                    //ToDo
                    break;

                default:
                    container.Style = GetFontStyle(clazz.ToLower());
                    break;
            }

            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "class":
                    case "id":
                        break;
                    case "style":
                        container.Color = new Color(attribute.Value);
                        break;
                    case "title":
                        if (container is Footnote fn)
                        {
                            if (fn.IsToolTip)
                                fn.Add(SplitRaw(attribute.Value));
                            else
                                fn.Title = attribute.Value;
                        }
                        else
                            throw new NotImplementedException();
                        break;
                    case "lang":
                        //Language
                        break;
                    default:
                        throw new NotImplementedException();
                }

            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    case "font-style":
                        container.Style = GetFontStyle(prop.Value);
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
                    case "class":
                        switch (attribute.Value)
                        {
                            case "pageref":
                                link.IsPageRef = true;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;

                    case "target":
                        //ToDo?
                        break;

                    default:
                        throw new NotImplementedException();
                }

            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    case "float":
                        //ToDo?
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return link;
        }
        private ContainerElement GetSuperSub(Token token)
        {
            SuperIndex super = new SuperIndex();
            switch (token.MyKind)
            {
                case Token.Kind.Super:
                    break;
                case Token.Kind.Sub:
                    super.IsSub = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "class":
                        switch (attribute.Value.ToLower())
                        {
                            case "fract":
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return super;
        }
        private ContainerElement GetQuote(Token token)
        {
            Quote bq = new Quote
            {
                IsBlock = token.MyKind == Token.Kind.BlockQuote
            };
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "class":
                        switch (attribute.Value.ToLower())
                        {
                            case "note":
                                bq.Size -= 1;
                                break;
                            case "gray":
                                bq.Color = new Color(attribute.Value);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case "lang":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    default:
                        throw new NotImplementedException();
                }
            return bq;
        }
        private ContainerElement GetListItem(Token token)
        {
            var li = new ListItem();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "value":
                        li.Term = attribute.Value;
                        break;
                    case "class":
                        switch (attribute.Value.ToLower())
                        {
                            case "nostyle":
                                li.Term = "";
                                break;
                            case "just":
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    default:
                        throw new NotImplementedException();
                }
            return li;
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
                    break;
                case "dl":
                    list.IsDescriptional = true;
                    break;
                default:
                    throw new NotImplementedException();
            }

            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    case "type":
                    case "class":
                        list.SetNumbering(attribute.Value);
                        break;
                    case "style":
                        list.SetNumbering(attribute.Value);
                        break;
                    case "start":
                        list.StartNumber = attribute.Value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    case "font-style":
                        list.Style = GetFontStyle(prop.Value);
                        break;
                    case "margin-left":
                        list.LeftMargin = prop.ValueAsMeasure();
                        break;
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
                    case "class":
                    case "style":
                        switch (attribute.Value.ToLower())
                        {
                            case "smallcaps":
                                header.Style = new Style() { IsSmallCaps = true };
                                break;
                            default:
                                header.SetInfo(attribute.Value);
                                break;
                        }
                        break;
                    case "align":
                    case "id":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
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
                        table.Alignment = GetAlignment(attribute.Value);
                        break;
                    case "valign":
                        //ToDo?
                        break;
                    case "border":
                        table.Border = attribute.ValueAsDouble();
                        break;
                    case "bordercolor":
                        //ToDo
                        break;
                    case "padding":
                    case "cellpadding":
                        table.Padding = attribute.ValueAsDouble();
                        break;
                    case "spacing":
                    case "cellspacing":
                        table.Spacing = attribute.ValueAsDouble();
                        break;
                    case "class":
                        switch (attribute.Value)
                        {
                            case "poem":
                            case "vers":
                                table.IsPoem = true;
                                break;
                            case "box":
                            case "centerbox":
                                table.IsBox = true;
                                break;
                            case "truetop":
                                //ToDo?
                                break;
                            default:
                                table.Alignment = GetAlignment(attribute.Value);
                                break;
                        }
                        break;
                    case "summary":
                        table.MyCaption = new Table.Caption(attribute.Value);
                        break;
                    case "width":
                        table.Width = attribute.ValueAsLength();
                        break;
                    case "frame":
                        switch (attribute.Value.ToLower())
                        {
                            case "box":
                            case "border":
                                table.IsBox = true;
                                break;
                            case "void":
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        break;
                    case "rules":
                        table.SetBorderStyle(attribute.Value);
                        break;
                    case "height":
                    case "dir":
                    case "cols":
                    case "background":
                        break;
                    default:
                        throw new NotImplementedException();
                }

            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    case "border-top":
                        //ToDo?
                        //dotted
                        break;
                    case "border-right":
                        //ToDo?
                        break;
                    case "border-bottom":
                        //ToDo?
                        break;
                    case "border-left":
                        //ToDo?
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
                    case "align":
                        table.Alignment = GetAlignment(attribute.Value.ToLower());
                        break;
                    case "valign":
                        table.VAlignment = GetVAlignment(attribute.Value.ToLower());
                        break;
                    case "class":
                    case "bgcolor":
                        //ToDo?
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
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
                        datum.VAlignment = GetVAlignment(attribute.Value.ToLower());
                        break;
                    case "colspan":
                        datum.ColSpan = (int)attribute.ValueAsDouble();
                        break;
                    case "rowspan":
                        datum.RowSpan = (int)attribute.ValueAsDouble();
                        break;
                    case "width":
                        datum.Width = attribute.ValueAsLength();
                        break;
                    case "height":
                        datum.Height = attribute.ValueAsLength();
                        break;
                    case "bgcolor":
                        //ToDo
                        break;
                    case "nowrap":
                        break;
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    case "border-bottom-width":
                        //ToDo
                        break;
                    case "border-bottom-style":
                        //ToDo
                        break;
                    default:
                        throw new NotImplementedException();
                }
            return datum;
        }
        private Table.RowContainer GetTableRowContainer(Token token)
        {
            Table.RowContainer rc = new Table.RowContainer();
            switch (token.MyKind)
            {
                case Token.Kind.TableHead:
                    rc.MyKind = Table.RowContainer.Kind.Head;
                    break;
                case Token.Kind.TableBody:
                    rc.MyKind = Table.RowContainer.Kind.Body;
                    break;
                case Token.Kind.TableFoot:
                    rc.MyKind = Table.RowContainer.Kind.Foot;
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
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    default:
                        throw new NotImplementedException();
                }
            return rc;
        }
        private Table.Caption GetCaption(Token token)
        {
            var caption = new Table.Caption();
            foreach (var attribute in token.Attributes)
                switch (attribute.Name.ToLower())
                {
                    default:
                        throw new NotImplementedException();
                }
            foreach (var prop in token.StyleProperties)
                switch (prop.Name)
                {
                    default:
                        throw new NotImplementedException();
                }
            return caption;
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

        public static int GetAlignment(string value)
        {
            switch (value.ToLower())
            {
                case "":
                case "poem":
                case "allname":
                case "rolle":
                case "wide":
                case "bündig":
                case "long":
                case "einr":
                case "einr1":
                case "calibre13":
                case "volume":
                case "first":
                case "repliccont":
                case "footnote":
                case "blockquote":
                case "preface":
                case "noindent":
                case "indented":
                case "indent":
                case "lat":
                case "paul simmel":
                case "hanging":
                case "c1781":
                case "‹h3":
                case "glossar":
                case "part":
                case "tb":
                case "fall":
                case "small":
                case "regie":
                case "smallcaps":
                case "not":
                case "fntext":
                case "chupter":
                case "section":
                case "real":
                case "anm":
                case "anmerk":
                case "font110":
                case "prosa":
                case "left":
                case "left0":
                case "rleft":
                case "lewft":
                case "ldeft":
                case "leftmarg":
                case "leftmarg2":
                case "leftmrg":
                case "leftjust":
                case "abstract":
                case "abstract2":
                case "abstract3":
                case "letter":
                case "drama":
                case "drma":
                case "drammarg":
                case "drama1":
                case "drama2":
                case "cdrama":
                case "cdrama1":
                case "cdrama2":
                case "vinit":
                case "intial":
                case "intital":
                case "initial":
                case "inital":
                case "initital":
                case "intitial":
                case "stage":
                case "scene":
                case "toc":
                case "chor":
                case "chormarg":
                case "titlepage":
                case "titel_3":
                case "line":
                case "rzeile":
                case "lzeile":
                case "iniline":
                case "characters":
                case "justify":
                case "def":
                case "reg":
                case "western":
                case "v":
                case "d":
                case "weihe":
                case "recipient":
                case "sender":
                case "speaker":
                case "act":
                case "lektorat":
                case "p5":
                case "p20":
                case "p23":
                case "p24":
                case "p25":
                case "el":
                case "mid":
                case "de":

                case "versmarg":

                case "truetop":
                case "top":
                    return 0;

                case "caption":
                case "dblmargr":
                case "dblmarg":
                case "dbkmarg":
                case "dllmarg":
                case "dlbmarg":
                case "dblamrg":
                case "dlmargr":
                case "dlmarg":
                case "dlamrg":
                case "stars":
                case "chapter":
                case "motto":
                case "motto50":
                case "note":
                case "center":
                case "center0":
                case "centr":
                case "center\"\"":
                case "cent":
                case "cente":
                case "cebter":
                case "cemter":
                case "enter":
                case "denter":
                case "centersm":
                case "centersmall":
                case "centersml":
                case "centerbig":
                case "centerbib":
                case "figure":
                case "figcation":
                case "figcaptio":
                case "figcaption":
                case "true":
                case "ture":
                case "box"://???
                case "centerbox":
                case "kasten":
                case "bigtable":

                case "absmiddle":
                case "middle":
                    return 1;

                case "right":
                case "riight":
                case "riht":
                case "rightmarg":
                case "signature":
                case "sinature":
                case "signatur":
                case "date":
                case "dedication":
                case "epigraph":
                case "address":

                case "absbottom":
                case "bottom":
                case "baseline":
                    return 2;

                default:
                    throw new NotImplementedException();
            }
        }
        public static int GetVAlignment(string value)
        {
            switch (value.ToLower())
            {
                case "top":
                    return 0;

                case "center":
                case "middle":
                    return 1;

                case "bottom":
                    return 2;

                default:
                    throw new NotImplementedException();
            }
        }
        public static Style GetFontStyle(string propValue)
        {
            switch (propValue)
            {
                case "small-caps":
                    return new Style() { IsSmallCaps = true };
                case "normal":
                    return new Style();
                case "lower":
                    return new Style() { IsLower = true };
                case "upper":
                    return new Style() { IsUpper = true };
                case "smallcaps":
                    return new Style() { IsSmallCaps = true };
                case "underline":
                    return new Style() { IsUnderlined = true };
                case "overline":
                    return new Style() { IsOverlined = true };
                case "spaced":
                case "wide":
                case "superwide":
                    return new Style() { IsWide = true };
                case "fraktur":
                    return new Style() { IsFraktur = true };
                case "tt":
                    return new Style() { IsMonoSpace = true };
                case "durch":
                    return new Style() { IsCrossedOut = true };
                default:
                    throw new NotImplementedException();
            }
        }
        public static int GetSize(string value)
        {
            switch (value.ToLower())
            {
                case "80%":
                case "small":
                    return -1;
                case "normal":
                    return 0;
                case "big":
                    return 1;
                default:
                    throw new NotImplementedException();
            }
        }
        //private static double GetMeasure(string measure)
        //{
        //    measure = measure.Trim().ToLower();
        //    if (measure.Substring(measure.Length - 2) == "em")
        //        return double.Parse(measure.Substring(0, measure.Length - 2));
        //    if (measure.Substring(measure.Length - 2) == "cm")
        //        return double.Parse(measure.Substring(0, measure.Length - 2));
        //    throw new NotImplementedException();
        //}
    }
}
