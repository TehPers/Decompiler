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
            this.Code = code;

            // Add all the edges
            foreach (Instruction cur in code) {
                if (cur.OpCode.FlowControl != FlowControl.Branch && cur.Next != null)
                    this.Add(new Edge<Instruction>(cur, cur.GetNextInCode(code)));
                if (cur.Operand is Instruction branch)
                    this.Add(new Edge<Instruction>(cur, branch.GetThisOrNextInCode(code)));
            }
        }
    }
}
