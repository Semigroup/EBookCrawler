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
            public int VAlignment { get; set; }
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
            public int Alignment { get; set; }
            public int VAlignment { get; set; }

            public void Add(IEnumerable<TextElement> data)
            {
                foreach (var item in data)
                    if (item is Datum datum)
                        this.Data.Add(datum);
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
                        this.Rows.Add(row);
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
                this.Add(new Word() { Value = summary });
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
        public int Alignment { get; set; }
        public BorderStyle Style { get; set; } = Table.BorderStyle.All;

        //ToDo
        public Length Width { get; set; } = new Length() { Value = 1, IsProportional = true };
        public double Padding { get; set; }
        public double Spacing { get; set; }
        public double Border { get; set; }
        public bool IsPoem { get; set; }

        public void Add(IEnumerable<TextElement> rows)
        {
            foreach (var item in rows)
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
            List<int> alignments = new List<int>();
            foreach (var cont in Segments)
                foreach (var row in cont.Rows)
                    for (int i = 0; i < row.Data.Count; i++)
                        if (alignments.Count <= i)
                            alignments.Add(row.Data[i].Alignment);
            string sep = ((Style & BorderStyle.Columns) != 0) ? "|" : "";

            writer.Write(@"{" + sep);
            foreach (var al in alignments)
            {
                switch (al)
                {
                    case 0:
                        writer.Write("l");
                        break;
                    case 1:
                        writer.Write("c");
                        break;
                    case 2:
                        writer.Write("r");
                        break;
                    default:
                        throw new NotImplementedException();
                }
                writer.Write(sep);
            }
            writer.WriteLine("}");
        }
        protected void WriteTabular(LatexWriter writer)
        {
            bool rowBorder = (Style & BorderStyle.Rows) != 0;

            writer.Write(@"\begin{tabular}");
            WriteColAlignment(writer);
            if (rowBorder)
                writer.WriteLine(@"\hline");
            foreach (var cont in Segments)
                cont.ToLatex(writer, rowBorder);

            writer.WriteLine(@"\end{tabular}");
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
            writer.WriteLine(@"\begin{table}[]");
            writer.WriteAlignment(Alignment);
            WriteTabular(writer);
            WriteCaption(writer);
            writer.WriteLine(@"\end{table}[]");
        }
    }
}
