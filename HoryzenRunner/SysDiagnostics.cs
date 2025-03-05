using System;

namespace HoryzenRunner
{
    public class SysDiagnostics
    {
        public static void Wait(int ms)
        {
            Task.Delay(ms).Wait();
        }
    }
}