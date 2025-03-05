using HoryzenRunner;
using System;
using System.IO;

class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting execution...");
        //Task.Delay(1000).Wait();
        Console.Clear();

        SysIO.Write(">System booted", Path.Join(SysFS.CurrentOS_Directory, "log.txt"));

        SysConsole.Writeln("Basic OS\n\n\n");
        SysGraphics.CreateBox("Introduction", "Welcome to your F-OS! Use the Boot.img.cs to edit this start up and modify any behaviors\n\nHappy coding! :D");
        SysConsole.Writeln("\n\n\n");
        SysIO.Append(">Showed Intro screen", Path.Join(SysFS.CurrentOS_Directory, "log.txt"));
        SysDiagnostics.Wait(1000);

        SysIO.Append(">System exiting", Path.Join(SysFS.CurrentOS_Directory, "log.txt"));
        return;
    }
}
