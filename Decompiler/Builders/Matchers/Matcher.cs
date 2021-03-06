﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teh.Decompiler.Builders;
using Teh.Decompiler.Graphs;

namespace Teh.Decompiler.Builders.Matchers {
    public abstract class Matcher {

        public abstract bool Matches(MatcherData data);

        public abstract void Build(CodeWriter writer, MatcherData data);

        public class MatcherData {
            public MethodDefinition Method { get; }
            public TypeNamer Namer { get; }
            public Queue<Instruction> Code { get; set; }
            public Stack<string> Stack { get; set; } = new Stack<string>();
            public ILGraph Graph => new ILGraph(Code ?? new Queue<Instruction>());

            public MatcherData(MethodDefinition method, TypeNamer namer, IEnumerable<Instruction> instructions) {
                this.Method = method;
                this.Namer = namer;
                this.Code = new Queue<Instruction>(instructions);
            }
        }
    }
}
