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
            Publisher
        }
        public Info MyInfo { get; set; } = Info.None;
        public int Hierarchy { get; set; }

        public void SetInfo(string value) {
            switch (value.ToLower())
            {
                case "author":
                    this.MyInfo = Info.Author;
                    break;
                case "pseudo":
                    this.MyInfo = Info.Pseudonym;
                    break;
                case "title":
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
                case "western":
                case "ce":
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
