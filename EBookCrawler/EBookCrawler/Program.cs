using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    class Program
    {
        static readonly string libRoot = "E:\\GutenbergLibrary\\";

        static void Main(string[] args)
        {
            //LoadWebLibrary();
            TranscriptBooks("fabeln");
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

            foreach (var name in books)
            {
                var found = orga.Library.FindBook(name).ToArray();
                for (int i = 0; i < found.Length; i++)
                    found[i].WriteBook(libRoot);
            }
        }
    }
}
