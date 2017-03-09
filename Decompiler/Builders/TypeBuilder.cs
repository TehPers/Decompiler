using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders {
    public class TypeBuilder : IBuilder {
        public TypeDefinition Type { get; }

        public TypeBuilder(TypeDefinition type) {
            this.Type = type;
        }

        public void Build(CodeWriter writer) {
            // Information
            writer.WriteLine($"// Assembly: {Type.Module.Assembly.Name}");
            writer.WriteLine($"// Module: {Type.Module.Name}");
            writer.WriteLine($"// Type: {Type.Name}");

            // Namespace
            writer.WriteLine($"namespace {Type.Namespace}");
            writer.WriteLine("{");
            writer.AddIndent();

            // Modifiers
            writer.WriteIndent();
            if (Type.IsNotPublic) writer.Write("private ");
            if (Type.IsPublic) writer.Write("public ");
            if (Type.IsAbstract) writer.Write("abstract ");
            if (Type.IsSealed) writer.Write("sealed ");

            // Class name
            writer.Write($"class {Type.Name}");
            writer.WriteLine();

            // Opening brace - Type
            writer.WriteLine("{");
            writer.AddIndent();

            // Write each method
            TypeNamer namer = new TypeNamer();
            foreach (MethodDefinition method in Type.Methods) {
                MethodBuilder builder = new MethodBuilder(method, namer);
                builder.Build(writer);
                writer.WriteLine();
            }

            // Closing brace - Type
            writer.RemoveIndent();
            writer.WriteLine("}");

            // Closing brace - Namespace
            writer.RemoveIndent();
            writer.WriteLine("}");
        }
    }
}
