using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace EBookCrawler
{
    class Program
    {
        static string libRoot = "E:\\GutenbergLibrary\\";
        static string latexOutput = @"D:\Github\EBookCrawler\build\";

        static void ShowHelp()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("EBookCrawler needs to be run with one of the following commands:");
            Console.WriteLine();
            Console.WriteLine("EBookCrawler init <path>                     Creates a new Library at the specified path.");
            Console.WriteLine("                                             This command will scrape all works at https://www.projekt-gutenberg.org/info/texte/allworka.html");
            Console.WriteLine("                                             and save the html pages at <path>.");
            Console.WriteLine("                                             Use this command if you want to update your current library.");
            Console.WriteLine("                                             Note: Depending on your internet connection");
            Console.WriteLine("                                             this command can take 30 mins up to several hours.");
            Console.WriteLine();
            Console.WriteLine("EBookCrawler books <string> <path1> <path2>  Will transcript each work whose title contains the given string.");
            Console.WriteLine("                                             <path1> needs to be the path where the library was saved,");
            Console.WriteLine("                                             i.e., <path1> must be the same argument that was given in init.");
            Console.WriteLine("                                             <path2> specifies where the new .tex files shall be saved.");
            Console.WriteLine();
            Console.WriteLine("EBookCrawler author <string> <path1> <path2> Will transcript each work whose author's name contains the given string.");
            Console.WriteLine("                                             <path1> needs to be the path where the library was saved,");
            Console.WriteLine("                                             i.e., <path1> must be the same argument that was given in init.");
            Console.WriteLine("                                             <path2> specifies where the new .tex files shall be saved.");
        }

        static void UpdateLibrary()
        {
            Logger.LogInfo("Creating Index of Website");
            LoadWebLibrary();
            Logger.LogInfo("Downloading Website");
            DownloadWebsite();
            Logger.LogInfo("Finished Updating/Creating Library");
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            switch (args[0])
            {
                case "init":
                    libRoot = args[1];
                    UpdateLibrary();
                    break;

                case "books":
                    libRoot = args[2];
                    latexOutput = args[3];
                    TranscriptBooks(args[1]);
                    break;
                case "author":
                    libRoot = args[2];
                    latexOutput = args[3];
                    TranscriptBooksOfAuthor(args[1]);
                    break;
                default:
                    ShowHelp();
                    break;
            }
        }

        static void LoadWebLibrary()
        {
            Organizer orga = new Organizer(libRoot);
            orga.DownloadLibrary();
            orga.SaveLibrary();
        }
        static void DownloadWebsite()
        {
            Organizer orga = new Organizer(libRoot);
            orga.LoadLibrary();
            orga.DownloadWebPage(false);
        }
        static void TranscriptBooks(string book)
        {
            Console.WriteLine("Transcripting texts with \"" + book + "\" in their title.");
            Console.WriteLine("Using library at " + libRoot + " and producing output to " + latexOutput + ".");

            Organizer orga = new Organizer(libRoot);
            orga.LoadLibrary();

            var found = orga.Library.FindBook(book).ToArray();
            Console.WriteLine("Found the following text titles:");
            foreach (var title in found)
                Console.WriteLine(title);

            for (int i = 0; i < found.Length; i++)
                found[i].WriteLatex(libRoot, latexOutput);
        }
        static void TranscriptBooksOfAuthor(string author)
        {
            author = author.ToLower();

            Console.WriteLine("Transcripting texts whose author's name contains \"" + author + "\".");
            Console.WriteLine("Using library at " + libRoot + " and producing output to " + latexOutput + ".");


            Organizer orga = new Organizer(libRoot);
            orga.LoadLibrary();

            foreach (var tuple in orga.Library.Authors)
                if (tuple.Key.ToLower().Contains(author))
                {
                    Console.WriteLine("Found author: " + tuple.Key);
                    foreach (var book in tuple.Value.Books)
                        book.Value.WriteLatex(libRoot, latexOutput);
                }
        }
    }
}
