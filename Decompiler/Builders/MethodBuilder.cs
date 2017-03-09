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

        public MethodBuilder(MethodDefinition method) {
            this.Method = method;
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
                if (Method.ReturnType.FullName == "System.Void") writer.Write("void ");
                else writer.Write(Method.ReturnType.FullName + " ");

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
                select param.ParameterType.Name + " " + param.Name
                )));
            writer.Write(")");

            writer.WriteLine();

            // IL code
            writer.WriteLine("{");
            writer.AddIndent();
            foreach (Instruction il in Method.Body.Instructions) {
                writer.WriteLine("//" + il.ToString());
            }

            // Decompiled code
            

            writer.RemoveIndent();
            writer.WriteLine("}");
        }
    }
}
