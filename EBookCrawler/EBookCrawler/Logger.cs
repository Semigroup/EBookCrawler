using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    public static class Logger
    {
        public static bool ShowWarnings { get; set; } = false;
        public static bool ShowErrors { get; set; } = true;

        public static void LogWarning(object obj)
        {
            if (ShowWarnings)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Error.WriteLine(obj.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        public static void LogError(object obj)
        {
            if (ShowErrors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine(obj.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
