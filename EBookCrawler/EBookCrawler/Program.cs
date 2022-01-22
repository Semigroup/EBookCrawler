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
            TranscriptBooks();
        }

        static void LoadWebLibrary()
        {
            Organizer orga = new Organizer();
            orga.DownloadLibrary();
            orga.SaveLibrary(libRoot);
        }
        static void TranscriptBooks()
        {
            Organizer orga = new Organizer();
            orga.LoadLibrary(libRoot);
        }
    }
}
