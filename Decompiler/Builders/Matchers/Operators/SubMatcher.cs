using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Operators {
    public class SubMatcher : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            string b = data.Stack.Pop();
            string a = data.Stack.Pop();
            data.Stack.Push($"({a} - {b})");
            data.Instructions.Dequeue();
        }

        public override bool Matches(MatcherData data) {
            return data.Instructions.Peek().OpCode == OpCodes.Sub;
        }
    }
}
