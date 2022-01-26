using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler.Texting
{
    public class Header : ContainerElement
    {
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
            Caption
        }
        public Info MyInfo { get; set; } = Info.None;
        public int Hierarchy { get; set; }

        public void SetInfo(string value) {
            switch (value.ToLower())
            {
                case "autor":
                case "autho":
                case "author":
                case "authpr":
                    this.MyInfo = Info.Author;
                    break;
                case "pseudo":
                    this.MyInfo = Info.Pseudonym;
                    break;
                case "title":
                case "titla":
                    this.MyInfo = Info.Title;
                    break;
                case "subtitle":
                case "subtitle02:10 28.02.2011":
                    this.MyInfo = Info.SubTitle;
                    break;
                case "chapter":
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
                    break;
                case "translator":
                    this.MyInfo = Info.Translator;
                    break;
                case "figcaption":
                    this.MyInfo = Info.Caption;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
