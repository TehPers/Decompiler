using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teh.Decompiler.Builders {
    public interface IBuilder {
        
        void Build(CodeWriter writer);

    }
}
