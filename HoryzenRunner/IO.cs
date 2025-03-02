using System;

namespace HoryzenRunner
{
    public static class SysIO
    {
        public static string osDir = Program.VHD;
        public static void Initialize()
        {
            osDir = Program.VHD;
            if (!Directory.Exists(osDir)) throw new Exception("OS Dir not initialised - KERNEL PANIC");
        }

        public static void Write(object value, string filepath)
        {
            throw new Exception("FS: "+ filepath);
        }
    }
}