using System;
using System.Runtime;
using System.IO.Compression;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            // Parse code into a SyntaxTree
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Join(CurrentOS_Directory, "Boot.img.cs")));

            var netCoreDir = Path.GetDirectoryName(typeof(object).Assembly.Location);
            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(Path.Combine(netCoreDir, "System.Private.CoreLib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(netCoreDir, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(netCoreDir, "System.Console.dll")),
                MetadataReference.CreateFromFile(Path.Combine(netCoreDir, "System.IO.FileSystem.dll")),
                MetadataReference.CreateFromFile(Assembly.GetExecutingAssembly().Location),
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            };



            CSharpCompilation compilation = CSharpCompilation.Create(
                "DynamicAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);
                if (!result.Success)
                {
                    Console.WriteLine("Compilation failed. Errors:");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                    throw new Exception("Dynamic compilation failed.");
                }

                var assembly = Assembly.Load(ms.ToArray());
                var myClassType = assembly.GetType("Program");
                if (myClassType == null)
                {
                    Console.WriteLine("Program class not found in the compiled assembly.");
                    return;
                }
                var greetMethod = myClassType.GetMethod("Main");
                if (greetMethod == null)
                {
                    Console.WriteLine("Main method not found.");
                    return;
                }
                greetMethod.Invoke(null, null);
            }
            /*
                Generally, the OS should have files that are;
                - boot.img.cs
                - boot.resources (or .resx idgaf)
             */
        }
    }
}