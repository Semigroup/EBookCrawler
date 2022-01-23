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
            public double VAlign { get; set; }
            //Ignore?
            public int ColSpan { get; set; } = 1;

            public void SetVAlignment(string attValue)
            {
                switch (attValue)
                {
                    case "center":
                        this.Alignment = 1;
                        break;
                    case "bottom":
                        this.Alignment = 2;
                        break;
                    case "top":
                        this.Alignment = 0;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public List<Row> Rows { get; set; } = new List<Row>();
        public double Padding { get; set; }
        public double Spacing { get; set; }
        public double Border { get; set; }
        public string Summary { get; set; }
        public int Alignment { get; set; }

        public void Add(IEnumerable<TextElement> rows)
        {
            foreach (var item in rows)
                if (item is Row row)
                    this.Rows.Add(row);
                else
                    throw new NotImplementedException();
        }

        public void SetAlignment(string attValue)
        {
            switch (attValue)
            {
                case "center":
                    this.Alignment = 1;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
