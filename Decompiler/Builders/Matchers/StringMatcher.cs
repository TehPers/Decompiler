using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace Teh.Decompiler.Builders.Matchers {
    public class StringMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            data.Stack.Push($"\"{data.Code.Dequeue().Operand as string}\"");
        }

        public override bool Matches(MatcherData data) {
            return data.Code.Peek().OpCode == OpCodes.Ldstr;
        }
    }
}
