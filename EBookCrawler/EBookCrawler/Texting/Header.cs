﻿using System;
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
            Caption,
            Note,
            Act,
            Scene,
            Dedication
        }
        public Info MyInfo { get; set; } = Info.None;
        public int Hierarchy { get; set; }

        public void SetInfo(string value) {
            switch (value.ToLower())
            {
                case "autor":
                case "autho":
                case "author":
                case "author-western":
                case "authot":
                case "authro":
                case "authpr":
                    this.MyInfo = Info.Author;
                    break;
                case "pseudo":
                case "pseunym":
                case "pseudonym":
                case "preudonym":
                    this.MyInfo = Info.Pseudonym;
                    break;
                case "title":
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
                case "subtitle":
                case "sutitle":
                case "subtitl":
                case "subtitletitle":
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
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
