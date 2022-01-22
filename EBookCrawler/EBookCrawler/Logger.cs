using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    public static class Logger
    {


        public static void LogLine(object obj)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(obj.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
