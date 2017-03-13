using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Fluff {
    public class StlocBrSLdlocMatcher : Matcher {
        public override void Build(CodeWriter writer, MatcherData data) {
            data.Code.Dequeue();
            data.Code.Dequeue();
            data.Code.Dequeue();
        }

        public override bool Matches(MatcherData data) {
            int? loc = data.Code.Peek().GetStLoc();
            if (loc != null) {
                Instruction[] instructions = data.Code.ToArray();
                if (instructions.Length < 3) // If there aren't enough instructions for this
                    return false;
                if (instructions[1].OpCode != OpCodes.Br_S) // If the second instruction isn't br.s
                    return false;
                if (instructions[1].Operand as Instruction != instructions[2]) // If the br.s doesn't point to the instruction right after it
                    return false;
                if (instructions[2].GetLdLoc() != loc) // If the var being loaded isn't the var that was stored
                    return false;

                // Make sure this var isn't necessary ahead in the code
                IEnumerable<Instruction> relevant = from i in data.Code
                                                    where i.GetStLoc() == loc || i.GetLdLoc() == loc
                                                    select i;

                foreach (Instruction i in relevant) {
                    if (i.GetStLoc() == loc) return true; // Local var changed before accessed again
                    if (i.GetLdLoc() == loc) return false; // Local var accessed again, therefore needed
                }

                return true;
            }

            return false;
        }
    }
}
