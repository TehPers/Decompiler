using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace Teh.Decompiler.Builders.Matchers {
    public class ReturnMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            data.Instructions.Dequeue();
            if (data.Stack.Count > 0) writer.WriteLine($"return {data.Stack.Pop()};");
            else if (data.Instructions.Any()) writer.WriteLine("return;");
        }

        public override bool Matches(MatcherData data) {
            return data.Instructions.Peek().OpCode == OpCodes.Ret;
        }
    }
}
