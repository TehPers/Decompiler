using System;
using System.Linq;

namespace Graphs {
    /// <summary>A simple, reusable edge class that does nothing fancy</summary>
    /// <typeparam name="TVertex">The type of vertex being stored</typeparam>
    public class Edge<TVertex> : IEdge<TVertex> where TVertex : class {
        public TVertex Source { get; }

        public TVertex Target { get; }

        /// <param name="source">The vertex this edge comes from</param>
        /// <param name="target">The vertex this ege points to</param>
        public Edge(TVertex source, TVertex target) {
            this.Source = source;
            this.Target = target;
        }

        public override string ToString() {
            return $"{this.Source.ToString()} -> {this.Target.ToString()}";
        }

        public override bool Equals(object obj) {
            if (obj is Edge<TVertex> edge)
                return this.Source.Equals(edge.Source) && this.Target.Equals(edge.Target);
            return false;
        }

        public override int GetHashCode() {
            return this.Source.GetHashCode() ^ this.Target.GetHashCode();
        }
    }
}
