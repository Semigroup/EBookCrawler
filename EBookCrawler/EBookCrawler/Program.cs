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

        static void Main(string[] args)
        {
            //LoadWebLibrary();

            TranscriptBooks("fabeln");

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
            Organizer orga = new Organizer();
            orga.DownloadLibrary();
            orga.SaveLibrary(libRoot);
        }
        static void TranscriptBooks(params string[] books)
        {
            Organizer orga = new Organizer();
            orga.LoadLibrary(libRoot);

            //foreach (var name in books)
            //{
            //    var found = orga.Library.FindBook(name).ToArray();
            //    for (int i = 0; i < found.Length; i++)
            //        found[i].WriteBook(libRoot);
            //}

            foreach (var author in orga.Library.Authors.Values)
                foreach (var book in author.Books.Values)
                    book.WriteBook(libRoot);
        }
    }
}
