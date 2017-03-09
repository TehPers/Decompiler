using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler {
    public class TypeNamer {

        public string GetName(TypeDefinition type) {
            return Aliases.ContainsKey(type) ? Aliases[type] : type.FullName;
        }

        public Dictionary<TypeDefinition, string> Aliases { get; } = new Dictionary<TypeDefinition, string>();
    }
}
