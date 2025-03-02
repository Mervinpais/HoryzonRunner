using System;
using System.Runtime.ExceptionServices;

namespace HoryzenRunner
{
    public static class SysGraphics
    {
        public static void CreateBox(string title, string message)
        {
            int width = Console.WindowWidth - 2;
            string titleLine = new string('─', width - 2 - title.Length);
            string horizontalLine = new string('─', width - 2 );

            Console.WriteLine($"┌{title + titleLine}┐");
            //Console.WriteLine($"│{title.PadRight(width - 2)}│");

            int messageLines = (int)Math.Ceiling((double)message.Length / (width - 2));
            for (int i = 0; i < messageLines; i++)
            {
                int startIdx = i * (width - 2);
                int length = Math.Min(width - 2, message.Length - startIdx);
                string line = message.Substring(startIdx, length);
                Console.WriteLine($"│{line.PadRight(width - 2)}│");
            }

            Console.WriteLine($"└{horizontalLine}┘");
        }
    }
}