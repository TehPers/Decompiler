using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Teh.Decompiler.Builders.Matchers.Constructs {
    public class DoWhile : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            Instruction next = data.Code.Peek();
            Instruction branch = data.Code.LastOrDefault(i => i.Operand == next);
            if (branch != null) {
                List<Instruction> loopCode = new List<Instruction>();
                while (data.Code.Peek() != branch)
                    loopCode.Add(data.Code.Dequeue());
                CodeBuilder builder = new CodeBuilder(data.Method, data.Namer, loopCode);
                builder.Data.Stack = data.Stack;
                writer.WriteLine("do");
                writer.WriteLine("{");
                writer.AddIndent();
                builder.Build(writer);
                writer.RemoveIndent();
                writer.WriteLine("}");

                data.Code.Dequeue();
                if (branch.OpCode == OpCodes.Brtrue || branch.OpCode == OpCodes.Brtrue_S)
                    writer.WriteLine($"while ({data.Stack.Pop()});");
                else if (branch.OpCode == OpCodes.Brfalse || branch.OpCode == OpCodes.Brfalse_S)
                    writer.WriteLine($"while (!{data.Stack.Pop()});");
                else {
                    string b = data.Stack.Pop();
                    string a = data.Stack.Pop();
                    if (branch.OpCode == OpCodes.Beq || branch.OpCode == OpCodes.Beq_S)
                        writer.WriteLine($"while ({a} == {b});");
                    else if (branch.OpCode == OpCodes.Bne_Un || branch.OpCode == OpCodes.Bne_Un_S)
                        writer.WriteLine($"while ({a} != {b});");
                    else if (branch.OpCode == OpCodes.Blt || branch.OpCode == OpCodes.Blt_S || branch.OpCode == OpCodes.Blt_Un || branch.OpCode == OpCodes.Blt_Un_S)
                        writer.WriteLine($"while ({a} < {b});");
                    else if (branch.OpCode == OpCodes.Ble || branch.OpCode == OpCodes.Ble_S || branch.OpCode == OpCodes.Ble_Un || branch.OpCode == OpCodes.Ble_Un_S)
                        writer.WriteLine($"while ({a} <= {b});");
                    else if (branch.OpCode == OpCodes.Bgt || branch.OpCode == OpCodes.Bgt_S || branch.OpCode == OpCodes.Bgt_Un || branch.OpCode == OpCodes.Bgt_Un_S)
                        writer.WriteLine($"while ({a} > {b});");
                    else if (branch.OpCode == OpCodes.Bge || branch.OpCode == OpCodes.Bge_S || branch.OpCode == OpCodes.Bge_Un || branch.OpCode == OpCodes.Bge_Un_S)
                        writer.WriteLine($"while ({a} >= {b});");
                }
            }
        }

        public override bool Matches(MatcherData data) {
            Instruction next = data.Code.Peek();
            if (data.Code.Any(i => i.Operand == next)) return true; // Found a branch pointing to this instruction
            return false;
        }
    }
}
