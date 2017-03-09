using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders {
    public class CodeBuilder : IBuilder {
        public List<Instruction> Instructions { get; }

        public CodeBuilder(IEnumerable<Instruction> instructions) {
            this.Instructions = instructions.ToList();
        }

        public void Build(CodeWriter writer) {

        }
    }
}
