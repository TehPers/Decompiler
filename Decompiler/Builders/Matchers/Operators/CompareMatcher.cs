using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Operators {
    public class CompareMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            Instruction i = data.Code.Dequeue();

            string op = null;
            if (i.OpCode == OpCodes.Cgt) op = ">";
            else if (i.OpCode == OpCodes.Cgt) op = "==";
            else if (i.OpCode == OpCodes.Cgt) op = "<";

            string b = data.Stack.Pop();
            string a = data.Stack.Pop();
            data.Stack.Push($"({a} {op} {b})");
        }

        public override bool Matches(MatcherData data) {
            Instruction i = data.Code.Peek();
            return i.OpCode == OpCodes.Cgt
                || i.OpCode == OpCodes.Ceq
                || i.OpCode == OpCodes.Clt;
        }
    }
}
