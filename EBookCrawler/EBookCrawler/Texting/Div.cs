using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Div : ContainerElement
    {
        public enum Kind
        {
            Unspecified,
            Letter,
            Figure,
            Chapter,
            Section,
            Motto,
            Text,
            Dedication,
            TitlePage,
            Characters,
            Speaker,
            Epigraph,
            Note,
            Caption,
            Table,
            Preface
        }
        public Kind MyKind { get; set; } = Kind.Unspecified;
        public void SetKind(string value)
        {
            this.SetClass(value);
            switch (value)
            {
                case "preface":
                    MyKind = Kind.Preface;
                    break;
                case "note":
                    MyKind = Kind.Note;
                    break;
                case "epigraph":
                    MyKind = Kind.Epigraph;
                    break;
                case "speaker":
                    MyKind = Kind.Speaker;
                    break;
                case "characters":
                    MyKind = Kind.Characters;
                    break;
                case "titlepage":
                    MyKind = Kind.TitlePage;
                    break;
                case "letter":
                    MyKind = Kind.Letter;
                    break;
                case "figcaption":
                case "figure":
                    MyKind = Kind.Figure;
                    break;
                    //MyKind = Kind.Caption;
                    //break;
                case "act":
                case "allname":
                case "chapter":
                case "chupter":
                    MyKind = Kind.Chapter;
                    break;
                case "motto":
                    MyKind = Kind.Motto;
                    break;
                case "font110":
                case "center":
                case "c1781":
                case "indented":
                case "right":
                case "wide":
                    MyKind = Kind.Text;
                    break;
                case "dedication":
                    MyKind = Kind.Dedication;
                    break;
                case "scene":
                case "section":
                case "part":
                case "volume":
                    MyKind = Kind.Section;
                    break;
                case "bigtable":
                    MyKind = Kind.Table;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
