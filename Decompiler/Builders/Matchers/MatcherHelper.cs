using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers {
    public static class MatcherHelper {

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
    }
}
