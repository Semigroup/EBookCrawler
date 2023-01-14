using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Header : ContainerElement
    {
        public enum Level
        {
            None = 0,
            TitlePage = 1,
            Part = 2,
            Chapter = 3,
            Section = 4,
            SubSection = 5,
            SubSubSection = 6,
            Paragraph = 7,
            SubParagraph = 8,
            Unspecified = 12
        }
        public enum Info
        {
            None,
            Author,
            Title,
            SubTitle,
            Chapter,
            Pseudonym,
            Date,
            Publisher,
            Translator,
            Caption,
            Note,
            Act,
            Scene,
            Dedication
        }

        public Info MyInfo { get; set; } = Info.None;
        public Level MyLevel { get; set; } = Level.Unspecified;
        public int Hierarchy { get; set; }

        public override bool HasExteriorEnvironment()
            => true;

        public void SetInfo(string value) {
            switch (value.ToLower())
            {
                case "author":
                case "authoer":
                case "autor":
                case "autho":
                case "author-western":
                case "authot":
                case "authro":
                case "authpr":
                case "name":
                    this.MyInfo = Info.Author;
                    break;
                case "pseudo":
                case "pseunym":
                case "pseudonym":
                case "psudonym":
                case "preudonym":
                    this.MyInfo = Info.Pseudonym;
                    break;
                case "title":
                case "tiel":
                case "titlte":
                case "tititle":
                case "titlöe":
                case "titla":
                case "titel":
                case "itle":
                case "tlte":
                case "tilte":
                    this.MyInfo = Info.Title;
                    break;
                case "sub":
                case "subtitle":
                case "sutitle":
                case "subtitl":
                case "subtitletitle":
                case "subtitle02:10 28.02.2011":
                    this.MyInfo = Info.SubTitle;
                    break;
                case "chapter":
                case "drama":
                    this.MyInfo = Info.Chapter;
                    break;
                case "date":
                    this.MyInfo = Info.Date;
                    break;
                case "publisher":
                    this.MyInfo = Info.Publisher;
                    break;
                case "center":
                case "centerbig":
                case "western":
                case "ce":
                case "vers":
                case "dblmarg":
                case "initial":
                    break;
                case "translator":
                    this.MyInfo = Info.Translator;
                    break;
                case "figcaption":
                    this.MyInfo = Info.Caption;
                    break;
                case "note":
                    this.MyInfo = Info.Note;
                    break;
                case "act":
                    this.MyInfo = Info.Act;
                    break;
                case "scene":
                    this.MyInfo = Info.Scene;
                    break;
                case "dedication":
                    this.MyInfo = Info.Dedication;
                    break;
                case "right":
                    this.MyAlignment = TextElement.Alignment.Right;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override void ToLatex(LatexWriter writer)
        {
            if (!IsVisible)
                return;
            writer.ForceWriteLine(1);
            
            writer.HeaderLevel = MyLevel;
            switch (MyLevel)
            {
                case Level.Part:
                case Level.Chapter:
                case Level.Section:
                case Level.SubSection:
                case Level.SubSubSection:
                case Level.Paragraph:
                case Level.SubParagraph:
                    var fns = ExtractFootnotes();
                    foreach (var fn in fns)
                        fn.IsVisible = false;

                    writer.Write(@"\");
                    writer.Write(GetCommand(MyLevel));
                    writer.Write(@"{");
                    base.ToLatex(writer);
                    writer.WriteLine(@"}");

                    foreach (var fn in fns)
                    {
                        fn.IsVisible = true;
                        fn.ToLatex(writer);
                    }

                    break;

                case Level.TitlePage:
                    writer.WriteBeginAlignment(Alignment.Center);
                    switch (MyInfo)
                    {
                        case Info.Pseudonym:
                        case Info.Author:
                        case Info.Translator:
                            writer.Write(@"\bfseries");
                            writer.Write(@"\Large");
                            writer.WriteColor(new Texting.Color(160, 160, 160));
                            break;
                        case Info.Title:
                            writer.Write(@"\bfseries");
                            writer.Write(@"\Huge");
                            break;
                        case Info.SubTitle:
                            writer.Write(@"\bfseries");
                            writer.Write(@"\large");
                            break;
                        case Info.Date:
                            writer.Write(@"\bfseries");
                            writer.Write(@"\large");
                            break;
                        case Info.Publisher:
                            writer.Write(@"\bfseries");
                            writer.Write(@"\large");
                            break;
                        case Info.Dedication:
                            writer.Write(@"\bfseries");
                            writer.Write(@"\large");
                            break;
                        default:
                            writer.Write(@"\bfseries");
                            writer.Write(@"\Large");
                            break;
                    }
                    base.ToLatex(writer);
                    writer.WriteEndAlignment(Alignment.Center);
                    writer.ForceWriteLine();
                    break;
                case Level.Unspecified:
                    writer.Write(@"{");
                    writer.Write(@"\noindent");
                    writer.Write(@"\bfseries");
                    writer.Write(@"\Large");
                    writer.Write(@"\vspace{0.2em}\\");

                    base.ToLatex(writer);
                    //writer.WriteLine(@"\vspace{0.5em}\\");
                    writer.WriteLine(@"}");
                    break;

                default:
                    throw new NotImplementedException();
            }
            writer.HeaderLevel = Level.None;

            writer.ForceWriteLine(1);
        }
        public static string GetCommand(Level level)
        {
            switch (level)
            {
                case Level.Part:
                    return "part";
                case Level.Chapter:
                    return "chapter";
                case Level.Section:
                    return "section";
                case Level.SubSection:
                    return "subsection";
                case Level.SubSubSection:
                    return "subsubsection";
                case Level.Paragraph:
                    return "paragraph";
                case Level.SubParagraph:
                    return "subparagraph";
                case Level.Unspecified:
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<Footnote> ExtractFootnotes()
        {
            foreach (var item in TextElements)
                if (item is Footnote fn)
                    yield return fn;
        }
    }
}
