using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Teh.Decompiler.Graphs;

namespace Teh.Decompiler.Builders.Matchers.Fluff {
    public class LocFluff : IFluffMatcher {
        public IEnumerable<Instruction> GetFluff(Matcher.MatcherData data) {
            ILGraph graph = data.Graph;

            HashSet<Instruction> fluff = new HashSet<Instruction>();
            foreach (Instruction stloc in data.Code) {
                int? loc = stloc.GetStLoc();
                if (loc == null) continue;

                // Check that it gets immediately loaded again
                // TODO: Might want to search for the ldloc instead of assuming it's the next instruction.
                //       Basically, if no instructions between stloc and ldloc use the stack, then use that ldloc.
                Instruction ldloc = stloc.Next;
                if (ldloc?.GetLdLoc() != loc) continue;

                // Search graph to see if this location is stored over before it's used
                HashSet<Instruction> visited = new HashSet<Instruction>();
                Dictionary<Instruction, HashSet<Instruction>> targets = graph.Targets;
                HashSet<Instruction> searching = new HashSet<Instruction>(targets[ldloc]);
                bool isFluff = true;
                while (searching.Any()) {
                    HashSet<Instruction> nextSearch = new HashSet<Instruction>();
                    if (searching.Any(i => i.GetLdLoc() == loc)) {
                        isFluff = false;
                        break;
                    } else {
                        foreach (Instruction i in searching.Where(i => i.GetStLoc() != loc)) {
                            foreach (Instruction target in targets[i]) {
                                if (visited.Contains(target)) continue;
                                nextSearch.Add(target);
                                visited.Add(target);
                            }
                        }
                    }
                    searching = nextSearch;
                }

                if (isFluff) {
                    fluff.Add(stloc);
                    fluff.Add(ldloc);
                }
            }

            return fluff;
        }
    }
}
