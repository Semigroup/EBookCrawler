using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class TextChapter : ContainerElement
    {
        public enum Kind
        {
            Unspecified,

            TitlePage,
            Introduction,
            Chapter,
            Glossary,

            SingleText,
        }
        public Chapter Chapter { get; set; }
        public Kind MyKind { get; set; } = Kind.Unspecified;
        public List<Header> SpecialHeaders { get; set; }
        public Header Title { get; set; }

        public override bool HasExteriorEnvironment()
            => true;

        public override void ToLatex(LatexWriter writer)
        {
            AnalyseHeaders();

            if (Title != null)
            {
                Title.ToLatex(writer);
                Title.IsVisible = false;
            }
            else
                writer.WriteLine(@"\newpage");
            base.ToLatex(writer);
            if (Title != null)
                Title.IsVisible = true;
        }

        public void AnalyseHeaders()
        {
            SpecialHeaders = new List<Header>();
            List<Header>[] headers = new List<Header>[9];
            for (int i = 0; i < headers.Length; i++)
                headers[i] = new List<Header>();
            foreach (var head in GetHeaders())
            {
                if (head.MyInfo != Header.Info.None)
                    SpecialHeaders.Add(head);
                headers[head.Hierarchy].Add(head);
            }

            if (Chapter.Part.Chapters.Length == 1)
            {
                this.MyKind = Kind.SingleText;
                return;
            }
            else if (Chapter.Number == 0)
            {
                if (SpecialHeaders.Find(h => h.MyInfo == Header.Info.Title) != null)
                    this.MyKind = Kind.TitlePage;
                else
                    this.MyKind = Kind.Chapter;
            }
            else
                this.MyKind = Kind.Chapter;

            switch (MyKind)
            {
                case Kind.TitlePage:
                    foreach (var item in GetHeaders())
                        item.MyLevel = Header.Level.TitlePage;
                    break;
                case Kind.Chapter:
                case Kind.Unspecified:
                    if (headers[3].Count == 1)
                        this.Title = headers[3][0];
                    else
                        this.Title = new Header() { Hierarchy = 3, MyLevel = Header.Level.Chapter };
                    for (int i = 0; i < headers.Length; i++)
                        if (i < 3 && headers[i].Count > 0)
                        {
                            if (i < this.Title.Hierarchy)
                                this.Title = headers[i][0];
                            else
                                Logger.LogError("[TextChapter.AnalyseHeaders] Unrecognized header structure!");
                        }
                        else if (i >= 3)
                            foreach (var item in headers[i])
                                item.MyLevel = (Header.Level)i;
                    break;
                case Kind.SingleText:
                    for (int i = 0; i < headers.Length; i++)
                        if (i <= 3 && headers[i].Count > 0)
                            if (i < 3 && headers[i].Count > 0)
                            {
                                if (i < this.Title.Hierarchy)
                                    this.Title = headers[i][0];
                                else
                                    Logger.LogError("[TextChapter.AnalyseHeaders] Unrecognized header structure!");
                            }
                            else if (i >= 3)
                            foreach (var item in headers[i])
                                item.MyLevel = (Header.Level)i;
                    break;

                case Kind.Introduction:
                case Kind.Glossary:
                default:
                    throw new NotImplementedException();
            }

        }
        private IEnumerable<Header> GetHeaders()
        {
            var iter = Iterate();
            foreach (var item in iter)
                if (item is Header head)
                    yield return head;
        }
        private IEnumerable<TextElement> Iterate()
        {
            IEnumerable<TextElement> traverse(TextElement element)
            {
                yield return element;
                if (element is ContainerElement parent)
                    foreach (var child in parent.TextElements)
                        foreach (var item in traverse(child))
                            yield return item;
            }
            return traverse(this);
        }
    }
}
