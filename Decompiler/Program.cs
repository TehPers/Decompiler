using Mono.Cecil;
using Mono.Cecil.Cil;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Teh.Decompiler {
    public class Program {
        public static void Main(string[] args) {
            string cd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string outputDir = Path.Combine(cd, "Disassembly");
            Directory.CreateDirectory(outputDir);

            string testLibraryPath = "C:\\Users\\Teh\\Documents\\Visual Studio 2015\\Projects\\UnsafeTest\\TestLibrary\\bin\\Debug\\TestLibrary.exe";

            AssemblyDefinition testAssembly = AssemblyDefinition.ReadAssembly(testLibraryPath);

            foreach (ModuleDefinition module in testAssembly.Modules) {
                foreach (TypeDefinition type in module.Types) {
                    string outputFile;
                    if (type.Name == "<Module>") outputFile = Path.Combine(outputDir, "Module.cs");
                    else outputFile = Path.Combine(outputDir, type.Name + ".cs");
                    using (StreamWriter swriter = new StreamWriter(outputFile)) {
                        CodeWriter writer = new CodeWriter(swriter);

                        // Information
                        writer.WriteLine("// Assembly: {0}", testAssembly.Name);
                        writer.WriteLine("// Module: {0}", module.Name);
                        writer.WriteLine("// Type: {0}", type.Name);

                        writer.WriteIndent();
                        if (type.IsNotPublic) writer.Write("private ");
                        if (type.IsPublic) writer.Write("public ");
                        if (type.IsAbstract) writer.Write("abstract ");
                        if (type.IsSealed) writer.Write("sealed ");

                        writer.Write("class {0}", type.Name);
                        writer.WriteLine();

                        writer.WriteLine("{");
                        writer.AddIndent();
                        foreach (MethodDefinition method in type.Methods) {
                            writer.WriteIndent();

                            // Access modifiers
                            if (method.IsPrivate) writer.Write("private ");
                            if (method.IsPublic) writer.Write("public ");
                            if (method.IsStatic) writer.Write("static ");

                            if (method.IsConstructor) {
                                // Type name (constructor)
                                writer.Write(type.Name);
                            } else {
                                // Return type
                                if (method.ReturnType.FullName == "System.Void") writer.Write("void ");
                                else writer.Write(method.ReturnType.FullName + " ");

                                // Name
                                writer.Write(method.Name);
                            }

                            // Generic types
                            if (method.IsGenericInstance) {
                                writer.Write("<");
                                writer.Write(string.Join(", ", (
                                    from param in method.GenericParameters
                                    let optional = param.IsOptionalModifier
                                    select param.Name
                                    )));
                                writer.Write(">");
                            }

                            // Parameter list
                            writer.Write("(");
                            writer.Write(string.Join(", ", (
                                from param in method.Parameters
                                select param.ParameterType.Name + " " + param.Name
                                )));
                            writer.Write(")");

                            writer.WriteLine();

                            // Code
                            writer.WriteLine("{");
                            writer.AddIndent();
                            foreach (Instruction il in method.Body.Instructions) {
                                writer.WriteLine("//" + il.ToString());
                            }

                            CodeBuilder builder = new CodeBuilder(method);
                            string[] statements = builder.generateCode();
                            foreach (string statement in statements) {
                                writer.WriteLine(statement);
                            }

                            writer.RemoveIndent();
                            writer.WriteLine("}");
                        }
                        writer.RemoveIndent();
                        writer.WriteLine("}");
                    }
                }
            }

            //reader.Assembly.MainModule.EntryPoint.Body.Instructions;

            //Console.ReadKey();
        }
    }
}
