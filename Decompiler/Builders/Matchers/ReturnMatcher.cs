using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace Teh.Decompiler.Builders.Matchers {
    public class ReturnMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            data.Code.Dequeue();
            if (data.Stack.Any()) writer.WriteLine($"return {data.Stack.Pop()};");
            //else writer.WriteLine("return;");
        }

        public override bool Matches(MatcherData data) {
            return data.Code.Peek().OpCode == OpCodes.Ret;
        }
    }
}
