using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Constructs {
    public class If : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            Instruction branch = data.Code.Dequeue();
            Instruction target = branch.GetBranchTarget();
            if (target != null) {
                if (branch.OpCode == OpCodes.Brtrue || branch.OpCode == OpCodes.Brtrue_S)
                    writer.WriteLine($"if (!{data.Stack.Pop()})");
                else if (branch.OpCode == OpCodes.Brfalse || branch.OpCode == OpCodes.Brfalse_S)
                    writer.WriteLine($"if ({data.Stack.Pop()})");
                else {
                    string b = data.Stack.Pop();
                    string a = data.Stack.Pop();
                    if (branch.OpCode == OpCodes.Beq || branch.OpCode == OpCodes.Beq_S)
                        writer.WriteLine($"if ({a} != {b})");
                    else if (branch.OpCode == OpCodes.Bne_Un || branch.OpCode == OpCodes.Bne_Un_S)
                        writer.WriteLine($"if ({a} == {b})");
                    else if (branch.OpCode == OpCodes.Blt || branch.OpCode == OpCodes.Blt_S || branch.OpCode == OpCodes.Blt_Un || branch.OpCode == OpCodes.Blt_Un_S)
                        writer.WriteLine($"if ({a} >= {b})");
                    else if (branch.OpCode == OpCodes.Ble || branch.OpCode == OpCodes.Ble_S || branch.OpCode == OpCodes.Ble_Un || branch.OpCode == OpCodes.Ble_Un_S)
                        writer.WriteLine($"if ({a} > {b})");
                    else if (branch.OpCode == OpCodes.Bgt || branch.OpCode == OpCodes.Bgt_S || branch.OpCode == OpCodes.Bgt_Un || branch.OpCode == OpCodes.Bgt_Un_S)
                        writer.WriteLine($"if ({a} <= {b})");
                    else if (branch.OpCode == OpCodes.Bge || branch.OpCode == OpCodes.Bge_S || branch.OpCode == OpCodes.Bge_Un || branch.OpCode == OpCodes.Bge_Un_S)
                        writer.WriteLine($"if ({a} < {b})");
                }

                writer.WriteLine("{");
                writer.AddIndent();

                List<Instruction> loopCode = new List<Instruction>();
                while (data.Code.Peek() != target)
                    loopCode.Add(data.Code.Dequeue());
                CodeBuilder builder = new CodeBuilder(data.Method, data.Namer, loopCode);
                builder.Data.Stack = data.Stack;
                builder.Build(writer);
                writer.RemoveIndent();
                writer.WriteLine("}");
            }
        }

        public override bool Matches(MatcherData data) {
            Instruction next = data.Code.Peek();
            Instruction target = next.GetBranchTarget();
            return target != null && target.Offset > next.Offset;
        }
    }
}
