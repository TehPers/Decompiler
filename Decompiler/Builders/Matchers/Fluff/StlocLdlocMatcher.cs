using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders.Matchers.Fluff {
    public class StlocLdlocMatcher : Matcher {

        public override void Build(CodeWriter writer, MatcherData data) {
            // Take off these two
            data.Instructions.Dequeue();
            data.Instructions.Dequeue();
        }

        public override bool Matches(MatcherData data) {
            int? loc = data.Instructions.Peek().GetStLoc();
            if (loc != null) {
                Instruction[] instructions = data.Instructions.ToArray();
                if (instructions.Length < 2 || loc != instructions[1].GetLdLoc()) return false;

                IEnumerable<Instruction> relevant = from i in data.Instructions
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
