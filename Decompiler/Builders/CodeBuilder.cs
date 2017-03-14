using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Teh.Decompiler.Builders.Matchers;
using Teh.Decompiler.Builders.Matchers.Fluff;
using Teh.Decompiler.Builders.Matchers.Constructs;
using Teh.Decompiler.Builders.Matchers.Operators;
using Teh.Decompiler.Builders.Matchers.Variables;
using Teh.Decompiler.Graphs;

namespace Teh.Decompiler.Builders {
    public class CodeBuilder : IBuilder {
        public Matcher.MatcherData Data { get; }
        public ILGraph Graph { get; }

        public CodeBuilder(MethodDefinition method, TypeNamer namer, IEnumerable<Instruction> code) {
            // Instructions should not include "nop"s because they do nothing (by definition)
            this.Data = new Matcher.MatcherData(method, namer, code);
            RemoveFluff();
            this.Graph = new ILGraph(this.Data.Code);
        }

        // Pass 1
        private void RemoveFluff() {
            // Run through each fluff matcher and remove the fluff returned
            foreach (IFluffMatcher fluffMatcher in Fluffs) {
                IEnumerable<Instruction> fluff = fluffMatcher.GetFluff(this.Data);
                this.Data.Code = new Queue<Instruction>(this.Data.Code.Except(fluff));

                foreach (Instruction instruction in this.Data.Code) {
                    instruction.Next = instruction.Next.GetThisOrNextInCode(this.Data.Code);
                    instruction.Previous = instruction.Previous.GetThisOrPreviousInCode(this.Data.Code);
                    if (instruction.Operand is Instruction target)
                        instruction.Operand = target.GetThisOrNextInCode(this.Data.Code);
                }
            }
        }

        public void Build(CodeWriter writer) {
            // TODO: 3 pass system
            // Pass 1: Remove any "fluff" from the code using fluff Matchers
            // Pass 2: Create a directed graph of the code, where each instruction is a node, each instruction points to the next, and each branch also points to where it branches to
            // Pass 3: Convert the code to something that looks like C# using Matchers

            /* Pass 2: Find cycles */
            IEnumerable<IEnumerable<Instruction>> cycles = this.Graph.GetCycles().Select(cycle => cycle.OrderBy(i => i.Offset));
            foreach (IEnumerable<Instruction> cycle in cycles) {
                writer.WriteLine($"// Found cycle: {string.Join(", ", cycle.Select(i => $"IL_{i.Offset.ToString("X4")}: {i.OpCode.Name}"))}");
            }

            /* Pass 3: Decompile */
            // Define local variables
            // TODO: Define these variables only when they're needed instead and give them better names
            /*foreach (VariableDefinition variable in Data.Method.Body.Variables) {
                writer.WriteLine($"{Data.Namer.GetName(variable.VariableType)} V_{variable.Index};");
            }*/

            while (this.Data.Code.Any()) {
                try {
                    IEnumerable<Matcher> possibleLoops = from loop in Constructs
                                                         where loop.Matches(this.Data)
                                                         select loop;

                    if (possibleLoops.Any()) {
                        if (possibleLoops.Skip(1).Any()) writer.WriteLine($"//(Multiple loops found, taking first)");
                        possibleLoops.First().Build(writer, Data);
                    } else {
                        IEnumerable<Matcher> possibleMatchers = from matcher in Matchers
                                                                where matcher.Matches(this.Data)
                                                                select matcher;

                        if (possibleMatchers.Skip(1).Any()) {
                            writer.WriteLine($"//{this.Data.Code.Dequeue().ToString()} (Multiple matchers found)");
                        } else if (!possibleMatchers.Any()) {
                            writer.WriteLine($"//{this.Data.Code.Dequeue().ToString()} (No matchers found)");
                        } else {
                            possibleMatchers.First().Build(writer, this.Data);
                        }
                    }
                } catch (Exception ex) {
                    writer.WriteLine($"//{this.Data.Code.Dequeue().ToString()} (An error was thrown)");
                    writer.WriteLine($"// Error: {ex.Message}");
                }
            }
        }

        public List<IFluffMatcher> Fluffs { get; } = new List<IFluffMatcher>() {
            new NopFluff(),
            new BrFluff(),
            new LocFluff()
        };

        public List<Matcher> Constructs { get; } = new List<Matcher>() {
            new DoWhile(),
            new If()
        };

        public List<Matcher> Matchers { get; } = new List<Matcher>() {
            new SimpleMatcher(),
            new ReturnMatcher(),
            new BinaryOperatorMatcher(),
            new CallMatcher(),
            new NotMatcher(),
            new ArgsMatcher(),
            new IntMatcher()
        };
    }
}
