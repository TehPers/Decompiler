using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teh.Decompiler.Builders.Matchers;
using Teh.Decompiler.Builders.Matchers.Operators;
using Teh.Decompiler.Builders.Matchers.Variables;

namespace Teh.Decompiler.Builders {
    public class CodeBuilder : IBuilder {
        public Matcher.MatcherData Data { get; }

        public CodeBuilder(MethodDefinition method, TypeNamer namer, IEnumerable<Instruction> instructions) {
            this.Data = new Matcher.MatcherData(method, namer, instructions.Where(i => i.OpCode != OpCodes.Nop));
        }

        public void Build(CodeWriter writer) {
            while (Data.Instructions.Any()) {
                IEnumerable<Matcher> possibleMatchers = from matcher in Matchers
                                                               where matcher.Matches(Data)
                                                        select matcher;

                if (possibleMatchers.Count() > 1) {
                    writer.WriteLine($"//{Data.Instructions.Dequeue().ToString()} (Multiple matchers found)");
                } else if (!possibleMatchers.Any()) {
                    writer.WriteLine($"//{Data.Instructions.Dequeue().ToString()} (No matchers found)");
                } else {
                    possibleMatchers.First().Build(writer, Data);
                }
            }
        }

        public static List<Matcher> Matchers { get; } = new List<Matcher>() {
            new StringMatcher(),
            new PopMatcher(),
            new ReturnMatcher(),
            new CallMatcher(),
            new AddMatcher(),
            new SubMatcher(),
            new MulMatcher(),
            new DivMatcher(),
            new ArgsMatcher()
        };
    }
}
