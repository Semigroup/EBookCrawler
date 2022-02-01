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

        public override void ToLatex(LatexWriter writer)
        {
            //ToDo
            writer.WriteLine(@"\chapter{Kapitel " + Chapter.Number + "}");

            AnalyseHeaders();

            base.ToLatex(writer);
        }

        public void AnalyseHeaders()
        {
            SpecialHeaders = new List<Header>();
            List<Header>[] headers = new List<Header>[10];
            for (int i = 0; i < headers.Length; i++)
                headers[i] = new List<Header>();
            foreach (var elt in Iterate())
                if (elt is Header hr)
                {
                    if (hr.MyInfo != Header.Info.None)
                        SpecialHeaders.Add(hr);
                    headers[hr.Hierarchy].Add(hr);
                }


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
