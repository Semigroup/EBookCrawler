using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookCrawler
{
    public static class Logger
    {
        public static bool ShowWarnings { get; set; } = true;
        public static bool ShowErrors { get; set; } = true;
        public static bool ShowInfo { get; set; } = true;

        public static void LogWarning(object obj)
        {
            if (ShowWarnings)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
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

        public static void LogInfo(object obj) 
        {
            if (ShowInfo)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Error.WriteLine(obj.ToString());
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }  
        public static void LogWarning(object sender, object obj)
        {
            LogWarning("[" + sender + "] " + obj.ToString());
        }
        public static void LogError(object sender, object obj)
        {
            LogError("[" + sender + "] " + obj.ToString());
        }
        public static void LogInfo(object sender, object obj)
        {
            LogInfo("[" + sender + "] " + obj.ToString());

        }
    }
}
