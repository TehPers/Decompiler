using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teh.Decompiler.Builders.Matchers;
using Teh.Decompiler.Builders.Matchers.Fluff;
using Teh.Decompiler.Builders.Matchers.Operators;
using Teh.Decompiler.Builders.Matchers.Variables;

namespace Teh.Decompiler.Builders {
    public class CodeBuilder : IBuilder {
        public Matcher.MatcherData Data { get; }

        public CodeBuilder(MethodDefinition method, TypeNamer namer, IEnumerable<Instruction> instructions) {
            // Instructions should not include "nop"s because they do nothing (by definition)
            this.Data = new Matcher.MatcherData(method, namer, instructions.Where(i => i.OpCode != OpCodes.Nop));
        }

        public void Build(CodeWriter writer) {
            // TODO: 3 pass system
            // Pass 1: Remove any "fluff" from the code using fluff Matchers
            // Pass 2: Create a directed graph of the code, where each instruction is a node, each instruction points to the next, and each branch also points to where it branches to
            // Pass 3: Convert the code to something that looks like C# using Matchers

            while (Data.Instructions.Any()) {
                try {
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
                } catch (Exception ex) {
                    writer.WriteLine($"//{Data.Instructions.Dequeue().ToString()} (An error was thrown)");
                    writer.WriteLine($"// Error: {ex.Message}");
                }
            }

            if (Data.Stack.Any()) writer.WriteLine($"// Leftover stack: {string.Join(", ", Data.Stack)}");
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
            new ArgsMatcher(),
            new IntMatcher(),
            new CompareMatcher(),
            new StlocLdlocMatcher(),
            new StlocBrSLdlocMatcher()
        };
    }
}
