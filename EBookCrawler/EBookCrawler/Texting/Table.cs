using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Table : TextElement
    {
        public class Datum : ContainerElement
        {
            //ToDo
            public Alignment VAlignment { get; set; } = Alignment.Unspecified;
            public int ColSpan { get; set; } = 1;
            public int RowSpan { get; set; } = 1;
            public Length Width { get; set; } = new Length() { Value = 1, IsProportional = true };
            public Length Height { get; set; } = new Length() { Value = 0, IsProportional = true };

            public override void ToLatex(LatexWriter writer)
            {
                //ToDo
                base.ToLatex(writer);
            }
        }
        public class Row : TextElement
        {
            public List<Datum> Data { get; set; } = new List<Datum>();
            public Alignment MyAlignment { get; set; } = Alignment.Unspecified;
            public Alignment VAlignment { get; set; } = Alignment.Unspecified;

            public void Add(IEnumerable<TextElement> data)
            {
                foreach (var item in data)
                    if (item is Datum datum)
                    {
                        this.Data.Add(datum);
                        this.WordCount += datum.WordCount;
                    }
                    else
                        throw new NotImplementedException();
            }

            public override void ToLatex(LatexWriter writer)
            {
                bool first = true;
                foreach (var item in Data)
                {
                    if (first)
                        first = false;
                    else
                        writer.Write(" & ");
                    item.ToLatex(writer);
                }
                writer.WriteLine(@"\\");
            }
        }

        public class RowContainer : TextElement
        {
            public enum Kind
            {
                Unspecified,
                Head,
                Body,
                Foot
            }

            public Kind MyKind { get; set; }
            public List<Row> Rows { get; set; } = new List<Row>();

            public RowContainer(Row row)
            {
                Rows.Add(row);
            }
            public RowContainer()
            {

            }

            public void Add(IEnumerable<TextElement> rows)
            {
                foreach (var item in rows)
                    if (item is Row row)
                    {
                        this.Rows.Add(row);
                        this.WordCount += row.WordCount;
                    }
                    else
                        throw new NotImplementedException();
            }
            public override void ToLatex(LatexWriter writer)
            {
                throw new NotImplementedException();
            }
            public void ToLatex(LatexWriter writer, bool rowLines)
            {
                foreach (var row in Rows)
                {
                    row.ToLatex(writer);
                    if (rowLines)
                    writer.WriteLine(@"\hline");
                }
            }
        }
        public class Caption : ContainerElement
        {
            public Caption()
            {

            }
            public Caption(string summary)
            {
                this.Add(new Word(summary));
            }
            public override void ToLatex(LatexWriter writer)
            {
                throw new NotImplementedException();
            }
        }

        public enum BorderStyle
        {
            None = 0,
            Columns = 1,
            Rows = 2,
            All = 3
        }

        public Caption MyCaption { get; set; }
        public List<RowContainer> Segments { get; set; } = new List<RowContainer>();
        public Alignment MyAlignment { get; set; } = Alignment.Unspecified;
        public BorderStyle Style { get; set; } = Table.BorderStyle.None;

        //ToDo
        public Length Width { get; set; } = new Length() { Value = 1, IsProportional = true };
        public double Padding { get; set; }
        public double Spacing { get; set; }
        public double Border { get; set; }
        public bool IsPoem { get; set; }

        public void Add(IEnumerable<TextElement> rows)
        {
            foreach (var item in rows)
            {
                if (item is Row row)
                    this.Segments.Add(new RowContainer(row));
                else if (item is RowContainer container)
                    this.Segments.Add(container);
                else if (item is Caption caption)
                {
                    if (this.MyCaption == null)
                        this.MyCaption = caption;
                    else
                        throw new NotImplementedException();
                }
                else
                    throw new NotImplementedException();
                this.WordCount += item.WordCount;
            }
        }
        public void SetBorderStyle(string value)
        {
            switch (value.ToLower())
            {
                case "none":
                    Style = BorderStyle.None;
                    break;
                case "cols":
                case "groups":
                    Style = BorderStyle.Columns;
                    break;
                case "rows":
                    Style = BorderStyle.Rows;
                    break;
                case "all":
                    Style = BorderStyle.All;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected void WriteColAlignment(LatexWriter writer)
        {
            //Alignment add(Alignment a, Alignment b)
            //{
            //    if (a == Alignment.Unspecified)
            //        return b;
            //    else
            //        return a;
            //}
            int n = 0;
            foreach (var cont in Segments)
                foreach (var row in cont.Rows)
                    n = Math.Max(n, row.Data.Count);

            //var alignments = new Alignment[n];
            //for (int i = 0; i < n; i++)
            //    alignments[i] = Alignment.Unspecified;
            //foreach (var cont in Segments)
            //    foreach (var row in cont.Rows)
            //        for (int i = 0; i < row.Data.Count; i++)
            //            alignments[i] = add(alignments[i], row.Data[i].MyAlignment);

            string sep = ((Style & BorderStyle.Columns) != 0) ? "|" : "";

            writer.Write(@"{" + sep);
            for (int i = 0; i < n; i++)
                writer.Write(@"X" + sep);
            //foreach (var al in alignments)
            //{
            //    switch (al)
            //    {
            //        case Alignment.Unspecified:
            //        case Alignment.Left:
            //            writer.Write("l");
            //            break;
            //        case Alignment.Center:
            //            writer.Write("c");
            //            break;
            //        case Alignment.Right:
            //            writer.Write("r");
            //            break;
            //        default:
            //            throw new NotImplementedException();
            //    }
            //    writer.Write(sep);
            //}
            writer.WriteLine("}");
        }
        protected void WriteTabular(LatexWriter writer)
        {
            bool rowBorder = (Style & BorderStyle.Rows) != 0;

            writer.Write(@"\begin{tabularx}");
            writer.Write(@"{\textwidth}");
            WriteColAlignment(writer);
            if (rowBorder)
                writer.WriteLine(@"\hline");
            foreach (var cont in Segments)
                cont.ToLatex(writer, rowBorder);

            writer.WriteLine(@"\end{tabularx}");
        }
        protected void WriteCaption(LatexWriter writer)
        {
            if (MyCaption != null)
            {
                writer.WriteLine(@"\caption{");
                MyCaption.ToLatex(writer);
                writer.Write(@"}");
            }
        }
        public override void ToLatex(LatexWriter writer)
        {
            writer.WriteLine(@"\begin{table}");
            writer.TabularDepth++;

            writer.WriteAlignment(MyAlignment);
            WriteTabular(writer);
            WriteCaption(writer);

            writer.TabularDepth--;
            writer.WriteLine(@"\end{table}");
        }
    }
}
