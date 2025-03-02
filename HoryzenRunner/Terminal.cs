using System;

namespace HoryzenRunner
{
    public static class SysConsole
    {
        public static void Write(string message)
        {
            Console.Write(message);
        }

        public static void Writeln(string message)
        {
            Console.WriteLine(message);
        }
        public static void Clear()
        {
            Console.Clear();
        }

        public static string Input(string queiry) {
            return Console.ReadLine();
        }
    }
}