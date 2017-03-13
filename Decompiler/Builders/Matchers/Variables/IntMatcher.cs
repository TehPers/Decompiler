using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Variables {
    public class IntMatcher : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            Instruction instruction = data.Code.Dequeue();
            int val;
            if (instruction.OpCode == OpCodes.Ldc_I4_0) val = 0;
            else if (instruction.OpCode == OpCodes.Ldc_I4_1) val = 1;
            else if (instruction.OpCode == OpCodes.Ldc_I4_2) val = 2;
            else if (instruction.OpCode == OpCodes.Ldc_I4_3) val = 3;
            else if (instruction.OpCode == OpCodes.Ldc_I4_4) val = 4;
            else if (instruction.OpCode == OpCodes.Ldc_I4_5) val = 5;
            else if (instruction.OpCode == OpCodes.Ldc_I4_6) val = 6;
            else if (instruction.OpCode == OpCodes.Ldc_I4_7) val = 7;
            else if (instruction.OpCode == OpCodes.Ldc_I4_8) val = 8;
            else val = Convert.ToInt32(instruction.Operand);
            data.Stack.Push(val.ToString());
        }

        public override bool Matches(MatcherData data) {
            Instruction next = data.Code.Peek();
            return next.OpCode == OpCodes.Ldc_I4_0
                || next.OpCode == OpCodes.Ldc_I4_1
                || next.OpCode == OpCodes.Ldc_I4_2
                || next.OpCode == OpCodes.Ldc_I4_3
                || next.OpCode == OpCodes.Ldc_I4_4
                || next.OpCode == OpCodes.Ldc_I4_5
                || next.OpCode == OpCodes.Ldc_I4_6
                || next.OpCode == OpCodes.Ldc_I4_7
                || next.OpCode == OpCodes.Ldc_I4_8
                || next.OpCode == OpCodes.Ldc_I4_S;
        }
    }
}
