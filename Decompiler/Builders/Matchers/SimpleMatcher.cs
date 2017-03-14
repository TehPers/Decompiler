using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teh.Decompiler.Builders.Matchers;

namespace Teh.Decompiler.Builders.Matchers {
    public class SimpleMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            Instruction next = data.Code.Dequeue();
            if (next.OpCode == OpCodes.Dup) {
                data.Stack.Push(data.Stack.Peek());
            } else if (next.OpCode == OpCodes.Pop) {
                writer.WriteLine($"{data.Stack.Pop()};");
            } else if (next.OpCode == OpCodes.Ldstr) {
                data.Stack.Push($"\"{data.Code.Dequeue().Operand as string}\"");
            } else if (next.OpCode == OpCodes.Starg || next.OpCode == OpCodes.Starg_S) {
                writer.WriteLine($"{(next.Operand as ParameterDefinition).Name} = {data.Stack.Pop()};");
            }
        }

        public override bool Matches(MatcherData data) {
            Instruction next = data.Code.Peek();
            return next.OpCode == OpCodes.Dup
                || next.OpCode == OpCodes.Pop
                || next.OpCode == OpCodes.Ldstr
                || next.OpCode == OpCodes.Starg
                || next.OpCode == OpCodes.Starg_S;
        }
    }
}
