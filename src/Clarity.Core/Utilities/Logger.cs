using System;

namespace Clarity.Core.Utilities 
{ 
    public static class Logger 
    { 
        public static void LogInfo(string message) 
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            Console.ResetColor();
        }

        public static void LogWarning(string message) 
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[WARN] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            Console.ResetColor();
        }

        public static void LogError(string message, Exception ex = null) 
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            if (ex != null)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ResetColor();
        }
    } 
}
