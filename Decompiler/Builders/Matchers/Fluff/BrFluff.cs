using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;

namespace Teh.Decompiler.Builders.Matchers.Fluff {
    public class BrFluff : IFluffMatcher {
        public IEnumerable<Instruction> GetFluff(Matcher.MatcherData data) {
            return data.Code.Where(i => i.Next != null && i.GetBranchTarget() == i.Next);
        }
    }
}
