using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Organizer orga = new Organizer();
            orga.LoadLibrary();
            Console.Read();
        }
    }
}
