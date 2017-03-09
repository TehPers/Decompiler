using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teh.Decompiler.Builders;

namespace Teh.Decompiler.Matchers {
    public interface IPatternMatcher : IBuilder {

        bool Matches();
        
    }
}
