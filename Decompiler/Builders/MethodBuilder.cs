using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders {
    public class MethodBuilder : IBuilder {
        public MethodDefinition Method { get; }
        public TypeNamer Namer { get; }

        public MethodBuilder(MethodDefinition method, TypeNamer namer) {
            this.Method = method;
            this.Namer = namer;
        }

        public void Build(CodeWriter writer) {
            writer.WriteIndent();

            // Access modifiers
            if (Method.IsPrivate) writer.Write("private ");
            if (Method.IsPublic) writer.Write("public ");
            if (Method.IsStatic) writer.Write("static ");

            if (Method.IsConstructor) {
                // Type name (constructor)
                writer.Write(Method.DeclaringType.Name);
            } else {
                // Return type
                writer.Write(Namer.GetName(Method.ReturnType) + " ");

                // Name
                writer.Write(Method.Name);
            }

            // Generic types
            if (Method.IsGenericInstance) {
                writer.Write("<");
                writer.Write(string.Join(", ", (
                    from param in Method.GenericParameters
                    let optional = param.IsOptionalModifier
                    select param.Name
                    )));
                writer.Write(">");
            }

            // Parameter list
            writer.Write("(");
            writer.Write(string.Join(", ", (    
                from param in Method.Parameters
                select this.Namer.GetName(param.ParameterType) + " " + param.Name
                )));
            writer.Write(")");

            writer.WriteLine();

            // Begin method body
            writer.WriteLine("{");
            writer.AddIndent();

            // IL code
            writer.WriteLine("/* IL */");
            foreach (Instruction il in Method.Body.Instructions) {
                writer.WriteLine("//" + il.ToString());
            }
            writer.WriteLine();

            // Decompiled code
            writer.WriteLine("/* DECOMPILED */");
            CodeBuilder builder = new CodeBuilder(this.Method, this.Namer, instructions: Method.Body.Instructions);
            builder.Build(writer);
            
            // End method body
            writer.RemoveIndent();
            writer.WriteLine("}");
        }
    }
}
