using System.IO.Compression;
using System.Threading;

namespace HoryzenRunner
{
    public static class SysFS
    {
        private static string VHD = Program.VHD;
        public static string? OS_name;
        public static string OS_dirs = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "F_Operating_systems"); // This line sets the OS_dirs to the "F_Operating_systems" directory
        public static string CurrentOS_Directory = Path.Join(OS_dirs, OS_name);
        public static void Initialize()
        {
            //if (OS_name == null) throw new Exception("Ensure name for OS has been given before initialization of file systems");
            if (!File.Exists(VHD))
            {
                throw new Exception("Ensure VHD for OS has been given before initialization of file systems");
            }

            //OS_dirs = Path.GetFileName(VHD);
            if (!Directory.Exists(OS_dirs))
            {
                Console.WriteLine($"Creating OSes Directory path... at {OS_dirs}");
                Directory.CreateDirectory(OS_dirs); // This line ensures the "F_Operating_systems" directory exists
            }
            if (!Directory.Exists(Path.Join(OS_dirs, OS_name)))
            {
                Directory.CreateDirectory(Path.Join(OS_dirs, OS_name));
                Directory.CreateDirectory(Path.Join(
                                            Path.Join(OS_dirs, OS_name),
                                            "OS_Data" //Holds the data for all OSes
                                            )
                                        );
            }
            CurrentOS_Directory = Path.Join(OS_dirs, OS_name, Path.GetFileNameWithoutExtension(VHD)); // for the ".zip"

        GetFiles:
            Console.WriteLine("\n" + CurrentOS_Directory + "\n");
            // Console.WriteLine(Directory.Exists(CurrentOS_Directory)+"\n");
            if (!Directory.Exists(CurrentOS_Directory)) //check if a directory exists or has files
            {
                Console.WriteLine(VHD);
                if (VHD.EndsWith(".zip"))
                {
                    ZipFile.ExtractToDirectory(VHD, Path.GetFullPath(CurrentOS_Directory), true);
                    goto GetFiles; //check again now
                }
                else
                {
                    throw new Exception("The file you have provided is invalid");
                }
            }

            // The OS folder can be found at Path.Join(OS_dirs, OS_name)

            //After init we passover to boot.cs
            return;
        }
    }
}