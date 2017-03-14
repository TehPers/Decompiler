using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Operators {
    public class BinaryOperatorMatcher : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            string b = data.Stack.Pop();
            string a = data.Stack.Pop();
            string op = "";

            Instruction instruction = data.Code.Dequeue();
            if (instruction.OpCode == OpCodes.Add) op = "+";
            else if (instruction.OpCode == OpCodes.Sub) op = "-";
            else if (instruction.OpCode == OpCodes.Mul) op = "*";
            else if (instruction.OpCode == OpCodes.Div) op = "/";
            else if (instruction.OpCode == OpCodes.Cgt) op = ">";
            else if (instruction.OpCode == OpCodes.Ceq) op = "==";
            else if (instruction.OpCode == OpCodes.Clt) op = "<";

            data.Stack.Push($"({a} {op} {b})");
        }

        public override bool Matches(MatcherData data) {
            Instruction next = data.Code.Peek();
            return next.OpCode == OpCodes.Add
                || next.OpCode == OpCodes.Sub
                || next.OpCode == OpCodes.Mul
                || next.OpCode == OpCodes.Div
                || next.OpCode == OpCodes.Cgt
                || next.OpCode == OpCodes.Ceq
                || next.OpCode == OpCodes.Clt;
        }
    }
}
