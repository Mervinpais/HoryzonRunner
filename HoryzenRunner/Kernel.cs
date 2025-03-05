using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace HoryzenRunner
{
    public class Kernel
    {
        DateTime startTime = DateTime.Now;
        public static string CurrentOS_Directory => SysFS.CurrentOS_Directory ?? string.Empty;
        public static void Initialize()
        {
            LoadBootFile();
            //Use GUI implementation (if there)
            // LoadTUI_Imp(); //If unavaliable, it will automatically be skipped for Text only mode
        }

        private static void LoadBootFile()
        {
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
        }
    }
}