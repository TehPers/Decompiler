using Graphs;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Graphs {
    public class ILGraph : Graph<Instruction> {
        public IEnumerable<Instruction> Code { get; }

        public ILGraph(IEnumerable<Instruction> code) {
            this.Code = code.OrderBy(i => i.Offset);

            // Add all the edges
            foreach (Instruction cur in code) {
                if (cur.OpCode.FlowControl != FlowControl.Branch && cur.Next != null)
                    this.Add(new Edge<Instruction>(cur, cur.GetNextInCode(code)));
                if (cur.Operand is Instruction branch)
                    this.Add(new Edge<Instruction>(cur, branch.GetThisOrNextInCode(code)));
            }
        }

        public IEnumerable<Instruction> GetReachableCode() {
            HashSet<Instruction> visited = new HashSet<Instruction>();
            HashSet<Instruction> checking = new HashSet<Instruction>(this.GetRoots());
            Dictionary<Instruction, HashSet<Instruction>> targets = this.Targets;

            while (checking.Any()) {
                foreach (Instruction i in checking) visited.Add(i);
                checking = new HashSet<Instruction>(
                    from i in checking
                    from target in targets[i]
                    where !visited.Contains(target)
                    select target
                    );
            }

            return visited;
        }

        public override IEnumerable<Instruction> GetRoots() {
            if (!Code.Any()) return Enumerable.Empty<Instruction>();
            return new Instruction[] { Code.First() };
        }
    }
}
