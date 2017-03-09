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
            // Create namer
            TypeNamer namer = new TypeNamer();
            
            // System types with custom keywords
            namer.Aliases[Type.Module.TypeSystem.Boolean.FullName] = "bool";
            namer.Aliases[Type.Module.TypeSystem.Byte.FullName] = "byte";
            namer.Aliases[Type.Module.TypeSystem.Char.FullName] = "char";
            namer.Aliases[Type.Module.TypeSystem.Double.FullName] = "double";
            namer.Aliases[Type.Module.TypeSystem.Int16.FullName] = "short";
            namer.Aliases[Type.Module.TypeSystem.Int32.FullName] = "int";
            namer.Aliases[Type.Module.TypeSystem.Int64.FullName] = "long";
            namer.Aliases[Type.Module.TypeSystem.Object.FullName] = "object";
            namer.Aliases[Type.Module.TypeSystem.SByte.FullName] = "bool";
            namer.Aliases[Type.Module.TypeSystem.Single.FullName] = "float";
            namer.Aliases[Type.Module.TypeSystem.String.FullName] = "string";
            namer.Aliases[Type.Module.TypeSystem.UInt16.FullName] = "ushort";
            namer.Aliases[Type.Module.TypeSystem.UInt32.FullName] = "uint";
            namer.Aliases[Type.Module.TypeSystem.UInt64.FullName] = "ulong";
            namer.Aliases[Type.Module.TypeSystem.Void.FullName] = "void";

            // This type
            namer.Aliases[this.Type.FullName] = this.Type.Name;

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
