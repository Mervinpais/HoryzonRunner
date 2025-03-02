using HoryzenRunner;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        SysConsole.Writeln("Hello world!");
        SysGraphics.CreateBox("Hi", "message");
        SysIO.Write("Hello world!", Path.Join(SysFS.CurrentOS_Directory, "hello.txt"));
    }
}
