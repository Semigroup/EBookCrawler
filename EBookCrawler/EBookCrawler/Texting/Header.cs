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
            Chapter
        }
        public Info MyInfo { get; set; } = Info.None;
        public int Hierarchy { get; set; }

        public void SetInfo(string value) {
            switch (value.ToLower())
            {
                case "author":
                    this.MyInfo = Info.Author;
                    break;
                case "title":
                    this.MyInfo = Info.Title;
                    break;
                case "subtitle":
                    this.MyInfo = Info.SubTitle;
                    break;
                case "chapter":
                    this.MyInfo = Info.Chapter;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
