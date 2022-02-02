using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EBookCrawler
{
    class Program
    {
        static readonly string libRoot = "E:\\GutenbergLibrary\\";
        static readonly string latexOutput = @"D:\Github\EBookCrawler\build\";

        static void Main(string[] args)
        {
            //LoadWebLibrary();

            TranscriptBooks("galgenlieder");
            TranscriptBooks("pitaval");

            //Test();
        }

        static void Test()
        {
            string path = @"D:\Github\EBookCrawler\EBookCrawler\Testing\RepairTest01.xml";
            string text = File.ReadAllText(path);
            var tokenizer = new Parsing.Tokenizer();
            tokenizer.Tokenize(text);
            var repairer = new Parsing.Repairer();
            repairer.Repair(text, tokenizer.Tokens);
            Console.ReadKey();
        }

        static void LoadWebLibrary()
        {
            Organizer orga = new Organizer(libRoot);
            orga.DownloadLibrary();
            orga.SaveLibrary();
        }
        static void TranscriptBooks(params string[] books)
        {
            Organizer orga = new Organizer(libRoot);
            orga.LoadLibrary();

            foreach (var name in books)
            {
                var found = orga.Library.FindBook(name).ToArray();
                for (int i = 0; i < found.Length; i++)
                    found[i].WriteLatex(libRoot, latexOutput);
            }

            //var authors = orga.Library.Authors.Values.ToArray();
            //for (int i = 0; i < authors.Length; i++)
            //{
            //    Console.WriteLine(i + " of " + authors.Length);
            //    Console.WriteLine(authors[i].ToString());
            //    foreach (var book in authors[i].Books.Values)
            //        foreach (var part in book.Parts)
            //            if (part.Part.Chapters != null)
            //                foreach (var ch in part.Part.Chapters)
            //                    ch.ParseChapter(libRoot);
            //    {

            //    }
            //{

            //}
            //book.WriteLatex(libRoot, latexOutput);
        //}
        }
    }
}
