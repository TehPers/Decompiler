using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler {
    public class TypeNamer {

        public string GetName(TypeDefinition type) {
            if (type.IsArray)
                return $"{GetName(type.GetElementType())}[]";
            return Aliases.ContainsKey(type.FullName) ? Aliases[type.FullName] : type.FullName;
        }

        public string GetName(TypeReference type) {
            if (type.IsArray)
                return $"{GetName(type.GetElementType())}[]";
            return Aliases.ContainsKey(type.FullName) ? Aliases[type.FullName] : type.FullName;
        }

        public Dictionary<string, string> Aliases { get; } = new Dictionary<string, string>();
    }
}
