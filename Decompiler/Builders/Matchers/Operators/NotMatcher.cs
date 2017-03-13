using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Operators {
    public class NotMatcher : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            data.Stack.Push($"!{data.Stack.Pop()}");
            data.Code.Dequeue();
        }

        public override bool Matches(MatcherData data) {
            return data.Code.Peek().OpCode == OpCodes.Not;
        }
    }
}
