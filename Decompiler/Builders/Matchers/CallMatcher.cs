using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using Mono.Cecil;

namespace Teh.Decompiler.Builders.Matchers {
    public class CallMatcher : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            Instruction call = data.Instructions.Dequeue();
            MethodReference reference = call.Operand as MethodReference;

            // Get passed parameters
            string[] args = new string[reference.Parameters.Count];
            for (int i = 0; i < reference.Parameters.Count; i++) {
                args[i] = data.Stack.Pop();
            }

            // TODO: Come up with a better naming scheme
            string built = $"{reference.DeclaringType.FullName}.{reference.Name}({string.Join(", ", args)})";
            if (reference.ReturnType.FullName == "System.Void") {
                writer.WriteLine($"{built};");
            } else {
                data.Stack.Push(built);
            }
        }

        public override bool Matches(MatcherData data) {
            return data.Instructions.Peek().OpCode == OpCodes.Call;
        }
    }
}
