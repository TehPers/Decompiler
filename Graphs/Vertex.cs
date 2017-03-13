using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs {
    /// <summary>A wrapper class for vertices that are value types.</summary>
    /// <typeparam name="T">The type being wrapped.</typeparam>
    public class Vertex<T> {
        public T Value { get; }

        public Vertex(T value) {
            this.Value = value;
        }
    }
}
