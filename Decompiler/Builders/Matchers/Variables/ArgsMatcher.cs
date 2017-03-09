using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Variables {
    public class ArgsMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            int arg;
            Instruction i = data.Instructions.Dequeue();
            if (i.OpCode == OpCodes.Ldarg_0) arg = 0;
            else if (i.OpCode == OpCodes.Ldarg_1) arg = 1;
            else if (i.OpCode == OpCodes.Ldarg_2) arg = 2;
            else if (i.OpCode == OpCodes.Ldarg_3) arg = 3;
            else
                arg = (int) i.Operand;

            if (data.Method.HasThis) arg--;
            if (arg == -1) data.Stack.Push("this");
            else data.Stack.Push(data.Method.Parameters[arg].Name);
        }

        public override bool Matches(MatcherData data) {
            Instruction instruction = data.Instructions.Peek();
            return instruction.OpCode == OpCodes.Ldarg_0
                || instruction.OpCode == OpCodes.Ldarg_1
                || instruction.OpCode == OpCodes.Ldarg_2
                || instruction.OpCode == OpCodes.Ldarg_3
                || instruction.OpCode == OpCodes.Ldarg_S;
        }
    }
}
