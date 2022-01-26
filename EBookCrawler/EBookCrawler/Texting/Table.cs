using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Table : TextElement
    {
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
        }
        public class Datum : ContainerElement
        {
            public int VAlignment { get; set; }
            //Ignore?
            public int ColSpan { get; set; } = 1;
            public int RowSpan { get; set; } = 1;
            public Length Width { get; set; } = new Length() { Value = 1, IsProportional = true };
            public Length Height { get; set; } = new Length() { Value = 0, IsProportional = true };
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
        }

        public enum BorderStyle
        {
            Columns,
            Rows,
            None,
            All
        }

        public List<RowContainer> Segments { get; set; } = new List<RowContainer>();
        public double Padding { get; set; }
        public double Spacing { get; set; }
        public double Border { get; set; }
        public string Caption { get; set; }
        public int Alignment { get; set; }
        public bool IsPoem { get; set; }
        public bool IsBox { get; set; }
        public Length Width { get; set; } = new Length() { Value = 1, IsProportional = true };
        public BorderStyle Style { get; set; } = Table.BorderStyle.All;

        public void Add(IEnumerable<TextElement> rows)
        {
            foreach (var item in rows)
                if (item is Row row)
                    this.Segments.Add(new RowContainer(row));
                else if (item is RowContainer container)
                    this.Segments.Add(container);
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
    }
}
