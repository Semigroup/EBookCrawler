using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBookCrawler.Parsing;

namespace EBookCrawler.Texting
{
    public class Footnote : ContainerElement
    {
        public enum Type
        {
            /// <summary>
            /// Inhalt ist Footnote
            /// </summary>
            FootNote,
            /// <summary>
            /// Inhalt ist Superindex. Footnote ist in Title
            /// Inhalte sollte nicht dargestellt werden...
            /// </summary>
            SideNote,
            /// <summary>
            /// Inhalt ist Wort, zu dem die Footnote gehört. Footnote ist in Title
            /// </summary>
            ToolTip
        }

        public Type MyType { get; set; } = Type.FootNote;
        /// <summary>
        /// Index: 1,*,...
        /// Oder Text der Footnote
        /// </summary>
        public string Title { get; set; }

        public Footnote()
        {
            this.Size = -2;
        }

        public override void ToLatex(LatexWriter writer)
        {
            switch (MyType)
            {
                case Type.FootNote:
                    writer.WriteLine(@"\footnote{");
                    base.ToLatex(writer);
                    writer.WriteLine(@"}");
                    break;
                case Type.SideNote:
                    writer.WriteLine(@"\footnote{");
                    writer.WriteText(Title);
                    writer.WriteLine(@"}");
                    break;
                case Type.ToolTip:
                    base.ToLatex(writer);
                    writer.WriteLine(@"\footnote{");
                    writer.WriteText(Title);
                    writer.WriteLine(@"}");
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
