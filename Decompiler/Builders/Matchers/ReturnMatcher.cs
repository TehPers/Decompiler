using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace Teh.Decompiler.Builders.Matchers {
    public class ReturnMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            string code = "return";
            if (data.Stack.Count > 0) code = $"{code} {data.Stack.Pop()}";
            writer.WriteLine($"{code};");
            data.Instructions.Dequeue();
        }

        public override bool Matches(MatcherData data) {
            return data.Instructions.Peek().OpCode == OpCodes.Ret;
        }
    }
}
