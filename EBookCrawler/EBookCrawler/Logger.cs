using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    public static class Logger
    {
        public static void LogWarning(object obj)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Error.WriteLine(obj.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void LogError(object obj)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(obj.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
