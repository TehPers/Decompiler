using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using Teh.Decompiler.Builders;

namespace Teh.Decompiler {
    public class Program {
        public static void Main(string[] args) {
            if (args == null || args.Length < 2)
                throw new ArgumentException($"Command line arguments: <input file> <output directory>");

            // Current directory
            string cd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Make sure input file exists
            string inputFile = Path.GetFullPath(Path.Combine(cd, args[0]));
            if (!File.Exists(inputFile))
                throw new ArgumentException("Input file does not exist!");

            // Create output directory
            string outputDir = Path.GetFullPath(Path.Combine(cd, args[1]));
            Directory.CreateDirectory(outputDir);

            // Load the assembly
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(inputFile);
            foreach (ModuleDefinition module in assembly.Modules) {
                foreach (TypeDefinition type in module.Types) {
                    string outputFile;
                    if (type.Name.Contains("<") || type.Name.Contains(">")) outputFile = Path.Combine(outputDir, $"{type.Name.Replace("<", "").Replace(">", "")}.special.cs");
                    else outputFile = Path.Combine(outputDir, type.Name + ".cs");

                    using (StreamWriter swriter = new StreamWriter(outputFile)) {
                        CodeWriter writer = new CodeWriter(swriter);

                        TypeBuilder builder = new TypeBuilder(type);
                        builder.Build(writer);
                    }
                }
            }

            //reader.Assembly.MainModule.EntryPoint.Body.Instructions;

            //Console.ReadKey();
        }
    }
}
