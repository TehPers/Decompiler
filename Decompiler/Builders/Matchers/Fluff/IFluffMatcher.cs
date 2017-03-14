using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Teh.Decompiler.Builders.Matchers.Matcher;

namespace Teh.Decompiler.Builders.Matchers.Fluff {
    public interface IFluffMatcher {
        IEnumerable<Instruction> GetFluff(MatcherData data);
    }
}
