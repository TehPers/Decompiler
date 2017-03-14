using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Teh.Decompiler {
    public static class InstructionHelper {

        public static int? GetStLoc(this Instruction i) {
            if (i.OpCode == OpCodes.Stloc_0) return 0;
            if (i.OpCode == OpCodes.Stloc_1) return 1;
            if (i.OpCode == OpCodes.Stloc_2) return 2;
            if (i.OpCode == OpCodes.Stloc_3) return 3;
            if (i.OpCode == OpCodes.Stloc_S) return Convert.ToInt32(i.Operand);
            return null;
        }

        public static int? GetLdLoc(this Instruction i) {
            if (i.OpCode == OpCodes.Ldloc_0) return 0;
            if (i.OpCode == OpCodes.Ldloc_1) return 1;
            if (i.OpCode == OpCodes.Ldloc_2) return 2;
            if (i.OpCode == OpCodes.Ldloc_3) return 3;
            if (i.OpCode == OpCodes.Ldloc_S) return Convert.ToInt32(i.Operand);
            return null;
        }

        public static Instruction GetBranchTarget(this Instruction i) {
            return i.Operand as Instruction;
        }

        /// <summary>Gets the first instruction in the code at or past this instruction</summary>
        public static Instruction GetThisOrNextInCode(this Instruction i, IEnumerable<Instruction> code) {
            if (i == null || code == null) return i;
            return code.FirstOrDefault(instruction => instruction.Offset >= i.Offset);
        }
        
        /// <summary>Gets the first instruction in the code past this instruction</summary>
        public static Instruction GetNextInCode(this Instruction i, IEnumerable<Instruction> code) {
            return code.FirstOrDefault(instruction => instruction.Offset > i.Offset);
        }

        /// <summary>Gets the last instruction in the code at or before this instruction</summary>
        public static Instruction GetThisOrPreviousInCode(this Instruction i, IEnumerable<Instruction> code) {
            if (i == null || code == null) return i;
            return code.LastOrDefault(instruction => instruction.Offset <= i.Offset);
        }

        /// <summary>Gets the last instruction in the code before this instruction</summary>
        public static Instruction GetPreviousInCode(this Instruction i, IEnumerable<Instruction> code) {
            if (i == null || code == null) return i;
            return code.LastOrDefault(instruction => instruction.Offset < i.Offset);
        }
    }
}
